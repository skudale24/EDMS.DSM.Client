using EDM.DataExport;
using EDM.PDFMappingVariables;
using EDMS.Data.Constants;
using EDMS.DSM.Server.Models;
using EDMS.DSM.Shared.Models;
using EDMS.Shared.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using VTI.Common;

namespace EDMS.DSM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<AuthenticationController> _logger;

        private ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Returns the Customer Communications
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [HttpGet("List/{programId}")]
        public async Task<IApiResult> GetCommunications(int programId)
        {
            var communications = _context.Database.SqlQueryRaw<Communication>($"EXECUTE dbo.[p_Get_HUP_AggregateList4CustomerCommunications]").ToList();
            return ApiResult<List<Communication>>.Success(communications);
        }

        /// <summary>
        /// Returns the Excel File in bytes
        /// TODO: Move the gemboxkey to configuration section
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("downloadexcelfile")]
        public async Task<IActionResult> DownloadExcelFile([FromBody] DownloadExcelFileRequest request)
        {
            try
            {
                DataSet dsExport = new DataSet();
                DataTable dtExport = new DataTable();
                if (request.ActionText.Equals("Generate Letters", StringComparison.OrdinalIgnoreCase))
                {
                    PDFGeneration objForExcel = new PDFGeneration();
                    objForExcel.ConnectionString = _configuration.GetConnectionString("Default");
                    objForExcel.ProgramId = request.ProgramId;
                    objForExcel.TemplateID = request.TemplateID;
                    objForExcel.LPCID = request.LPCID;
                    objForExcel.TemplateType = request.TemplateType;
                    dsExport = objForExcel.GetData();
                    if (!MsSql.IsEmpty(dsExport))
                    {
                        dtExport.Columns.Add("Application ID");
                        dtExport.Columns.Add("Applicant Name");
                        dtExport.Columns.Add("Letter Type");
                        dtExport.Columns.Add("LPC Name");
                        dtExport.Columns.Add("Date Generated");

                        foreach (DataRow dr in dsExport.Tables[0].Rows)
                        {
                            DataRow drExport = dtExport.NewRow();
                            drExport["Application ID"] = dr["ApplicationId"];
                            drExport["Applicant Name"] = dr["PrimaryApplicantName"].ToString().TrimEnd(',');
                            drExport["Letter Type"] = request.TemplateName;
                            drExport["LPC Name"] = request.CompanyName;
                            drExport["Date Generated"] = string.Empty;

                            dtExport.Rows.Add(drExport);
                        }
                        dsExport.Tables.Remove(dsExport.Tables[0]);
                        dsExport.Tables.Add(dtExport);
                    }

                }
                else
                {
                    EDM.CommunicationTemplate.CustomerCommunications objComTran = new EDM.CommunicationTemplate.CustomerCommunications();
                    dsExport = objComTran.GetApplicationDataByBatchId(request.BatchId);
                }

                if (!MsSql.IsEmpty(dsExport))
                {
                    dsExport.Tables[0].TableName = "CustomerList";
                    ExcelExport objExport = new ExcelExport(EDM.Setting.Module.AdminPortal);
                    objExport.Export(
                        dsExport,
                        "EMPZ-O95K-ELRO-I5T1",
                        $"CustomerList_{request.TemplateName}_{request.CompanyName}_{DateTime.Now.ToString("MMddyy")}",
                        new List<Tuple<String, String, String, String, String>>());

                    var l_sReader = System.IO.File.OpenRead(objExport.FilePath);
                    return (File(l_sReader, "application/octet-stream", Path.GetFileName(objExport.FilePath)));
                }

                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail("File not found."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }

        /// <summary>
        /// Generic method that downloads a file
        /// Returns file as byte array
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        [HttpGet("downloadsourcefile/{FileName}")]
        public async Task<IActionResult> DownloadSourceFile(string FileName)
        {
            try
            {
                string? uploadFilePath = _configuration["UploadFilePath"];

                var decodedFileName = WebUtility.UrlDecode(FileName).Replace("/", "\\");

                string filePath = Path.Join(uploadFilePath, decodedFileName);
                filePath = filePath.Replace("\\\\", "\\");

                var l_sReader = System.IO.File.OpenRead(filePath);
                return (File(l_sReader, "application/octet-stream", Path.GetFileName(filePath)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }

        /// <summary>
        /// Generates PDF of requested Letter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("generateletter")]
        public async Task<IActionResult> GenerateLetter([FromBody] GenerateLetterRequest request)
        {
            try
            {

                PDFGeneration obj = new PDFGeneration();
                obj.ConnectionString = _configuration.GetConnectionString("Default");
                obj.TemplateFile = request.TemplateFile;

                //NewLocalPath
                //obj.NewLocalPath = _configuration["UploadFilePath"];
                var contentRootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
                var path = "\\Tools\\CustomerCommunications\\ApplicationDoc\\";
                obj.NewLocalPath = contentRootPath + path;

                obj.ProgramId = request.ProgramId;
                obj.TemplateID = request.TemplateID;
                obj.LPCID = request.LPCID;
                obj.TemplateType = request.TemplateType;
                obj.GeneratedBy = request.GeneratedBy;
                obj.GetTemplatePDFGeneration();
                if (string.IsNullOrEmpty(obj.GeneratedFilePath))
                {
                    var msg = "File generation not successfull.";
                    return Ok(await ApiResult<PDFGeneration>.FailAsync(msg));
                }

                var GeneratedPath = obj.GeneratedFilePath;
                var BatchId = obj.BatchId;
                string letterType = Enum.GetName(typeof(ETemplateType), request.TemplateType);
                var OriginalName = DateTime.Now.ToString("yyyyMMdd") + "_CC" + letterType + ".pdf";

                return Ok(await ApiResult<PDFGeneration>.SuccessAsync(obj));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }

        /// <summary>
        /// Exports the Customer Communications grid data
        /// Returns excel file as byte array
        /// </summary>
        /// <returns></returns>
        [HttpGet("exportgrid")]
        public async Task<IActionResult> ExportGrid()
        {
            try
            {
                string sheetName = "Table1";

                ExcelExport objExport = new ExcelExport(SQLConstants.AdminPortal);

                DataSet DsExportData = new DataSet();
                DataSet ds = GetGridDataSet();
                if (!MsSql.IsEmpty(ds))
                {
                    DataTable dt = objExport.GetFilteredData(ds.Tables[0], string.Empty, "LPCID", sheetName);
                    if (dt != null)
                    {
                        DsExportData = objExport.SetFilteredDataSource(ds, dt, sheetName);
                    }
                }

                objExport.ExportListToExcel(SQLConstants.GemBoxKey, DsExportData, sheetName);

                var l_sReader = System.IO.File.OpenRead(objExport.FilePath);
                return (File(l_sReader, "application/octet-stream", Path.GetFileName(objExport.FilePath)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }

        private DataSet GetGridDataSet()
        {
            DataSet dsReturn = new DataSet();

            try
            {
                //EDM.User.Cookie _cook = new EDM.User.Cookie(Session);
                EDM.CommunicationTemplate.CustomerCommunications objComTran = new EDM.CommunicationTemplate.CustomerCommunications();

                objComTran.Module = SQLConstants.AdminPortal;
                objComTran.ProgramId = GridParams.ProgramID;
                dsReturn = objComTran.GetAllCustomerCommunication();
            }
            catch (Exception ex)
            {
                EDM.Common.Log.Error(SQLConstants.AdminPortal, "EDMS.DSM.Server.Controllers", "GetGridDataSet", ex);
            }
            return dsReturn;
        }

    }
}
