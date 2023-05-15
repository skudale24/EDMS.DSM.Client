using EDM.ContentHandler;
using EDMS.DSM.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using Telerik.Web.UI;
using VTI.Common;

namespace EDM.PDFMappingVariables
{
    public class PDFGeneration
    {
        #region --- Members ---
        public String Message = String.Empty;
        public string TemplateFile { get; set; }
        public string NewLocalPath { get; set; }
        public int? TemplateID { get; set; }
        public int? LPCID { get; set; }
        public string ConnectionString = string.Empty;

        public string ApplicationIDs = null;
        public string GeneratedFilePath { get; set; }
        public int? TemplateType { get; set; }
        public int? DocObjectType { get; set; }
        public string GeneratedFileName { get; set; }
        public int? GeneratedBy { get; set; }
        public string SystemName = null;//
        public string Storage = "";
        public byte[] localFile = null;
        public SqlDb Db;
        private String _configKey = String.Empty;
        public String Module;
        public int BatchId { get; set; }
        public String ConfigKey
        {
            get { return _configKey; }
            set
            {
                _configKey = value;
                Db = new SqlDb(_configKey);
            }
        }
        public long ProgramId;
        #endregion --- Members ---

        #region --- Constructors ---
        public PDFGeneration() { }
        public PDFGeneration(String module) : this() { Module = module; }
        #endregion --- Constructors

        #region --- Public Methods ---
        public string GetTemplatePDFGeneration()
        {
            try
            {
                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"ProgramId: {ProgramId}");
                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"GeneratedBy: {GeneratedBy}");

                EDM.Setting.DB.ConnectionString = ConnectionString;
                string TemplateURL = EDM.Setting.DB.GetByName(EDM.Setting.Key.ImageUrl,
                    ProgramId,
                    configKey: ConfigKey);
                String TemplatePath = TemplateURL + TemplateFile;
                String SaveAsFileName = String.Empty;
                DataSet dsTemplateFields = GetDataTemplateFields(); // mapping table dataset
                DataSet dsTemplate = GetData();// Template dataset from SP
                if (dsTemplate == null)
                {
                    Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", "No Records.");
                    return "No Records.";
                }
                List<byte[]> lstPages = new List<byte[]>();
                string DocumentTemplateMapping = dsTemplateFields.Tables[0].TableName;
                string DocumentTemplateSystemFields = dsTemplateFields.Tables[1].TableName;

                List<TemplateField> Fields = new List<TemplateField>();
                List<TemplateMapping> Mappings = new List<TemplateMapping>();

                foreach (DataRow row in dsTemplateFields.Tables[DocumentTemplateMapping].Rows)
                {
                    TemplateMapping mapping = new TemplateMapping();
                    mapping.MappingId = Convert.ToInt32(row["MappingId"]);
                    mapping.FieldExpression = Convert.ToString(row["FieldExpression"]);
                    mapping.TemplateVariable = Convert.ToString(row["TemplateVariable"]);
                    Mappings.Add(mapping);

                }
                foreach (DataRow row in dsTemplateFields.Tables[DocumentTemplateSystemFields].Rows)
                {
                    TemplateField field = new TemplateField();
                    field.FieldId = Convert.ToInt32(row["FieldId"]);
                    field.ColumnName = Convert.ToString(row["ColumnName"]);
                    field.QueryColumn = Convert.ToString(row["QueryColumn"]);
                    field.FieldType = Convert.ToString(row["FieldType"]);
                    Fields.Add(field);
                }
                List<int> ids = new List<int>();
                foreach (DataRow row in dsTemplate.Tables[0].Rows)
                {
                    var output = new MemoryStream();
                    var reader = new PdfReader(TemplatePath);
                    var stamper = new PdfStamper(reader, output);
                    var formFields = stamper.AcroFields;
                    foreach (var key in formFields.Fields.Keys)
                    {
                        TemplateMapping map = Mappings.Where(x => string.Equals(x.TemplateVariable.Trim(), key.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (map != null)
                        {
                            string fieldExpression = map.FieldExpression;
                            TemplateMapping tmpMap = GetFieldExpressionValue(row, map, Fields);
                            if (tmpMap != null && (!string.IsNullOrEmpty(tmpMap.FieldExpressionValue)) && tmpMap.ExpressionType != ControlType.Image && tmpMap.ExpressionType != ControlType.CheckBox && tmpMap.ExpressionType != ControlType.RadioButton)
                                formFields.SetField(key, tmpMap.FieldExpressionValue);
                            else if (tmpMap.ExpressionType.Equals(ControlType.Image, StringComparison.OrdinalIgnoreCase) && (!string.IsNullOrEmpty(tmpMap.FieldExpressionValue)))
                            {
                                PushbuttonField bt1 = formFields.GetNewPushbuttonFromField(key);
                                bt1.Layout = PushbuttonField.LAYOUT_ICON_ONLY;
                                bt1.ProportionalIcon = true;
                                PdfContentByte pdfContentByte = stamper.GetOverContent(1);
                                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(tmpMap.FieldExpressionValue);
                                bt1.Image = image1;
                                formFields.ReplacePushbuttonField(key, bt1.Field);
                            }
                            else if (tmpMap.ExpressionType.Equals(ControlType.CheckBox, StringComparison.OrdinalIgnoreCase) && (!string.IsNullOrEmpty(tmpMap.FieldExpressionValue)))
                            {
                                var reasons = tmpMap.FieldExpressionValue.Split('|');
                                if (reasons.Contains(key.Substring(key.Length - 1)))
                                {
                                    formFields.SetField(key.Substring(0, key.Length - 1) + key.Substring(key.Length - 1), "Yes", true);
                                }
                            }
                            else if (tmpMap.ExpressionType.Equals(ControlType.RadioButton, StringComparison.OrdinalIgnoreCase) && (!string.IsNullOrEmpty(tmpMap.FieldExpressionValue)))
                            {
                                int reasons = string.IsNullOrEmpty(tmpMap.FieldExpressionValue) ? 0 : Int32.Parse(tmpMap.FieldExpressionValue);
                                formFields.SetField(tmpMap.TemplateVariable, formFields.GetAppearanceStates(tmpMap.TemplateVariable)[reasons]);
                            }
                        }
                    }

                    stamper.FormFlattening = true;
                    stamper.Close();
                    byte[] content = output.ToArray();
                    lstPages.Add(content);
                    ids.Add(Convert.ToInt32(row["ApplicationId"]));

                    Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"ApplicationId: {row["ApplicationId"]}");

                }
                //Save file on server
                SaveAsFileName = (Guid.NewGuid().ToString()).Replace("-", "") + ".pdf";
                string newFile = NewLocalPath + SaveAsFileName;

                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"newFile: {newFile}");

                byte[] bytOutput = ConcatAndAddContent(lstPages);
                localFile = bytOutput;
                if (!Directory.Exists(Path.GetDirectoryName(newFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                }
                using (FileStream fs = File.Create(newFile))
                {
                    fs.Write(bytOutput, 0, (int)bytOutput.Length);
                }
                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"File Exists: {File.Exists(newFile)}");

                string RelLocation = GetUrlForUploadCustomerCommunicationsDocs();

                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"RelLocation: {RelLocation}");

                long DocTypeId = EDM.DocFile.Type.Application;
                FileFactory fileFactory = new FileHandlerCreator(Module);
                IFileHandler fileHndl = fileFactory.GetFileUploadInstance(DocTypeId, out Storage);
                bool isUploaded = fileHndl.UploadFile(newFile, RelLocation, SaveAsFileName);
                try
                {
                    if (File.Exists(newFile))
                    {
                        Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"Deleting file: {newFile}");
                        File.Delete(newFile);
                    }
                }
                catch (Exception ex)
                {

                    Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "GetTemplatePDFGeneration", ex);
                    Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", ex.Message);
                }

                // Set the parameters
                ApplicationIDs = string.Join(",", ids);
                GeneratedFilePath = RelLocation + SaveAsFileName;
                DocObjectType = Convert.ToInt32(DocTypeId);
                string enumText = Enum.GetName(typeof(ETemplateType), TemplateType);
                GeneratedFileName = DateTime.Now.ToString("yyyyMMdd") + "_CC" + enumText + ".pdf"; //2023 - 04 - 10_CC_AL;

                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"GeneratedFilePath: {GeneratedFilePath}");
                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"GeneratedFileName: {GeneratedFileName}");

                SystemName = SaveAsFileName;
                try
                {
                    SaveHUPCustomerCommunicationsResponse();
                }
                catch (Exception exception)
                {
                    Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "GetTemplatePDFGeneration", exception);
                    Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", $"SaveHUPCustomerCommunicationsResponse exception: {exception.Message}");
                }
                return "Document Generated";

            }
            catch (Exception exception)
            {
                Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "GetTemplatePDFGeneration", exception);
                Common.Log.Info("HUPCustomerCommunications", "GetTemplatePDFGeneration", exception.Message);
                return exception.Message;
            }
        }
        public static String GetUrlForUploadCustomerCommunicationsDocs() { return "/FilesUploaded/HUPApplication/CustomerCommunicationsDoc/"; }
        public static byte[] ConcatAndAddContent(List<byte[]> pdf)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.LETTER);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                foreach (byte[] p in pdf)
                {
                    reader = new PdfReader(p);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        doc.SetPageSize(PageSize.LETTER);
                        doc.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page, 0, 0);
                    }
                }

                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();
            }

            return all;
        }

        public DataSet GetData()
        {

            try
            {
                string SPName = null;
                switch (TemplateType)
                {
                    case (int?)ETemplateType._ApprovalLetter:
                        SPName = TemplateSP.ApprovalLetterSP;
                        break;
                    case (int?)ETemplateType._30DayNotification:
                        SPName = TemplateSP.ThirtyDayNotificationSP;
                        break;
                    case (int?)ETemplateType._IncompleteApplicationNotification:
                        SPName = TemplateSP.IncompleteApplicationNotificationSP;
                        break;
                    case (int?)ETemplateType._DeclarationofZeroIncomeNotification:
                        SPName = TemplateSP.DeclarationofZeroIncomeNotification;
                        break;
                    case (int?)ETemplateType._NoticeofIneligibility:
                        SPName = TemplateSP.NoticeOfIneligibilitySP;
                        break;
                    case (int?)ETemplateType._60DayNotification:
                        SPName = TemplateSP.SixtyDayNotificationSP;
                        break;
                    case (int?)ETemplateType._90DayNotification:
                        SPName = TemplateSP.NinetyDayNotificationSP;
                        break;
                }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["LPCID"] = LPCID;
                prms["TemplateId"] = TemplateID;
                String SqlforLog = string.Empty;

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ProgramID", SqlDbType.Int);
                parameters[0].Value = ProgramId;
                parameters[1] = new SqlParameter("@LPCID", SqlDbType.Int);
                parameters[1].Value = LPCID;
                parameters[2] = new SqlParameter("@TemplateID", SqlDbType.Int);
                parameters[2].Value = TemplateID;
                DataSet data = StoredProcedureExecutor.ExecuteStoredProcedureAsDataSet(ConnectionString, SPName, parameters);

                //String sql = MsSql.GetSqlStmt(SPName, prms, out SqlforLog);//p_GET_HUP_ApprovalLetterFields - previous
                //DataSet data = MsSql.ExecuteNoTransQuery(sql);
                return data;
            }
            catch (Exception exception)
            {
                Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "GetData", exception);
                Common.Log.Info("HUPCustomerCommunications", "GetData", exception.Message);
                return null;
            }
        }
        public DataSet GetDataTemplateFields()
        {

            try
            {
                Hashtable prms = new Hashtable();
                prms["TemplateID"] = TemplateID;
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DocumentTemplateSystemFields", prms, out SqlforLog);

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@TemplateID", SqlDbType.Int);
                parameters[0].Value = TemplateID;
                DataSet data = StoredProcedureExecutor.ExecuteStoredProcedureAsDataSet(ConnectionString, "p_GET_DocumentTemplateSystemFields", parameters);
                //DataSet data = MsSql.ExecuteNoTransQuery(sql);
                return data;
            }
            catch (Exception exception)
            {
                Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "GetDataTemplateFields", exception);
                Common.Log.Info("HUPCustomerCommunications", "GetDataTemplateFields", exception.Message);
                return null;
            }
        }

        public bool SaveHUPCustomerCommunicationsResponse()
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[10];
                Hashtable prms = new Hashtable();
                if (!string.IsNullOrEmpty(ApplicationIDs))
                {
                    prms["ApplicationIDs"] = ApplicationIDs;
                    // , @LPCID    BIGINT
                    // ,@GeneratedFilePath NVARCHAR(500)    
                    // ,@TemplateType INT
                    // , @GeneratedBy   BIGINT
                    // ,@DocObjectType SMALLINT = 12000
                    // , @GeneratedFileName NVARCHAR(150)
                    // ,@SystemName VARCHAR(128) = NULL
                    // ,@Storage VARCHAR(20)  = NULL
                }
                parameters[0] = new SqlParameter("@ApplicationIDs", SqlDbType.VarChar, 255);
                parameters[0].Value = ApplicationIDs;

                if (TemplateID != null)
                {
                    prms["TemplateID"] = TemplateID;
                }
                parameters[1] = new SqlParameter("@TemplateID", SqlDbType.Int);
                parameters[1].Value = TemplateID;

                if (LPCID != null)
                {
                    prms["LPCID"] = LPCID;
                }
                parameters[2] = new SqlParameter("@LPCID", SqlDbType.BigInt);
                parameters[2].Value = LPCID;

                if (!string.IsNullOrEmpty(GeneratedFilePath))
                {
                    prms["GeneratedFilePath"] = GeneratedFilePath;
                }
                parameters[3] = new SqlParameter("@GeneratedFilePath", SqlDbType.NVarChar, 150);
                parameters[3].Value = GeneratedFilePath;

                if (TemplateType != null)
                {
                    prms["TemplateType"] = TemplateType;
                }
                parameters[4] = new SqlParameter("@TemplateType", SqlDbType.Int);
                parameters[4].Value = TemplateType;

                if (GeneratedBy != null)
                {
                    prms["GeneratedBy"] = GeneratedBy;
                }
                parameters[5] = new SqlParameter("@GeneratedBy", SqlDbType.BigInt);
                parameters[5].Value = GeneratedBy;

                if (DocObjectType != null)
                {
                    prms["DocObjectType"] = DocObjectType;
                }
                parameters[6] = new SqlParameter("@DocObjectType", SqlDbType.SmallInt);
                parameters[6].Value = DocObjectType;

                if (!string.IsNullOrEmpty(GeneratedFileName))
                {
                    prms["GeneratedFileName"] = GeneratedFileName;
                }
                parameters[7] = new SqlParameter("@GeneratedFileName", SqlDbType.NVarChar, 150);
                parameters[7].Value = GeneratedFileName;

                if (!string.IsNullOrEmpty(SystemName))
                {
                    prms["SystemName"] = SystemName;
                }
                parameters[8] = new SqlParameter("@SystemName", SqlDbType.VarChar, 128);
                parameters[8].Value = SystemName;

                if (!string.IsNullOrEmpty(Storage))
                {
                    prms["Storage"] = Storage;
                }
                parameters[9] = new SqlParameter("@Storage", SqlDbType.VarChar, 20);
                parameters[9].Value = Storage;

                Common.Log.Info("HUPCustomerCommunications", "SaveHUPCustomerCommunicationsResponse", "p_A_HUP_CustomerCommunications called");
                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_A_HUP_CustomerCommunications", prms, out SqlforLog);
                Common.Log.Info("HUPCustomerCommunications", "SaveHUPCustomerCommunicationsResponse", SqlforLog);
                //DataSet ds = MsSql.ExecuteQuery(sql);
                DataSet ds = StoredProcedureExecutor.ExecuteStoredProcedureAsDataSet(ConnectionString, "p_A_HUP_CustomerCommunications", parameters);

                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error inserting Communication Transaction Data Transaction";
                    Common.Log.Info("HUPCustomerCommunications", "SaveHUPCustomerCommunicationsResponse", Message);
                    return false;
                }
                else
                {
                    BatchId = Int32.Parse(ds.Tables[0].Rows[0]["BatchID"].ToString());
                    return true;
                }

            }
            catch (Exception exception)
            {
                Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "SaveHUPCustomerCommunicationsResponse", exception);
                Common.Log.Info("HUPCustomerCommunications", "SaveHUPCustomerCommunicationsResponse", exception.Message);
                return false;
            }
        }

        public void DownloadFileFromServer(string generatedPath)
        {
            try
            {
                FileFactory fileFactory = new FileHandlerCreator(Module);
                IFileHandler fileHndl = fileFactory.GetFileDownloadInstance(Storage);
                bool fileExists = fileHndl.IsFileExists(generatedPath);

                if (fileExists)
                {
                    string letterType = Enum.GetName(typeof(ETemplateType), TemplateType);
                    byte[] generatedPDF = fileHndl.GetFile(generatedPath);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_CC" + letterType + ".pdf");
                    HttpContext.Current.Response.BinaryWrite(generatedPDF);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception exception)
            {
                Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "DownloadFileFromServer", exception);
                Common.Log.Info("HUPCustomerCommunications", "DownloadFileFromServer", exception.Message);
            }


        }

        #endregion --- Public Methods ---

        #region --- Private Methods ---

        private TemplateMapping GetFieldExpressionValue(DataRow dr, TemplateMapping map, List<TemplateField> TemplateFields)
        {
            try
            {

                Common.Log.Info("HUPCustomerCommunications", "GetFieldExpressionValue", "Get mapping field from the mapping Template");
                string resultValue = map.FieldExpression;
                char pipeChar = '|';
                map.ExpressionType = "String";
                List<string> fields = null;
                if (map.FieldExpression.Contains(pipeChar)) { fields = GetFields(map.FieldExpression); }
                else { fields = new List<string>(); fields.Add(map.FieldExpression); }
                fields.ForEach(field =>
                {
                    int key = Convert.ToInt32(field);
                    TemplateField tmpField = TemplateFields.Where(x => x.FieldId == key).FirstOrDefault();
                    if (tmpField != null)
                    {
                        string value = Convert.ToString(dr[tmpField.ColumnName]);
                        resultValue = map.FieldExpression.Contains(pipeChar) ? resultValue.Replace(string.Concat("|", key.ToString(), "|"), value) : value;

                    }
                    map.FieldExpressionValue = resultValue;
                    map.ExpressionType = tmpField.FieldType;
                });
            }
            catch (Exception ex)
            {
                Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "GetFieldExpressionValue", ex);
                Common.Log.Info("HUPCustomerCommunications", "GetFieldExpressionValue", ex.Message);
            }
            return map;

        }

        private List<string> GetFields(string expression)
        {
            try
            {
                string inputString = expression;
                char startSymbol = '|';
                char endSymbol = '|';
                List<string> results = new List<string>();

                int startIndex = inputString.IndexOf(startSymbol);
                while (startIndex != -1)
                {
                    int endIndex = inputString.IndexOf(endSymbol, startIndex + 1);
                    if (endIndex == -1) break;

                    string result = inputString.Substring(startIndex + 1, endIndex - startIndex - 1);
                    results.Add(result);

                    startIndex = inputString.IndexOf(startSymbol, endIndex + 1);
                }

                return results;
            }
            catch (Exception ex)
            {
                Common.Log.Error("HUPCustomerCommunications", "EDM.PDFMappingVariables", "GetFields", ex);
                Common.Log.Info("HUPCustomerCommunications", "GetFields", ex.Message);
                return null;
            }
        }

        #endregion --- Private Methods ---

    }
}
