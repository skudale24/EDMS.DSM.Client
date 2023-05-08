using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
//using System.Web;
using Telerik.Web.UI;

namespace EDM.DataExport
{
    public class ExcelExport
    {
        #region --- Properties ---

        public String Module = String.Empty;
        public String Message = String.Empty;
        public string FilePath = String.Empty;
        #endregion --- Properties ---

        #region --- Member Variables ---

        private string GemBoxKey = string.Empty;
        private string dateFormat = "mm/dd/yyyy HH:MM:ss AM/PM";
        private string shortDateFormat = "mm/dd/yyyy";
        #endregion --- Member Variables ---

        #region --- Constructors ---

        public ExcelExport()
        {

        }

        public ExcelExport(String module) : this()
        {
            Module = module;
        }

        #endregion --- Constructors ---

        #region --- Public Methods ---

        public void ExportToExcel(RadGrid uxRadGrid, string gemBoxKey, DataSet dsExportData, string sheetName, bool wrapText = false)
        {
            try
            {
                string NavUrlPath = string.Empty; //HttpContext.Current.Request.Path;
                sheetName = GetPlainFileName(sheetName);

                if (uxRadGrid == null || uxRadGrid.Items.Count == 0) return;
                if (dsExportData == null) return;
                if (dsExportData.Tables[sheetName] == null) return;
                int iColSeq = 0;
                DataTable dt = dsExportData.Tables[sheetName];
                //folowing list is as - ColumnName, Aggregate (ASP Format), DataFormatString  (ASP Format), FooterAggregateFormatString  (ASP Format), FooterText
                List<Tuple<String, String, String, String, String>> lstFormatedColumns = new List<Tuple<String, String, String, String, String>>();
                if (dt != null)
                {
                    EDM.Common.Log.Info(Module, "ExportToExcel", "## Step-7 : At start ExportToExcel function record count is = " + Convert.ToString(dt.Rows.Count) + ", FileName = " + sheetName);
                }

                foreach (GridColumn col in uxRadGrid.Columns)
                {
                    if (col.Visible)
                    {
                        var uniqueName = col.UniqueName == "UserCompanyName" ? "CompanyName" : col.UniqueName;
                        if (dt.Columns.Contains(uniqueName))
                        //if (dt.Columns.Contains(col.UniqueName))
                        {
                            dt.Columns[uniqueName].SetOrdinal(iColSeq);
                            if (dt.Columns.Contains(col.HeaderText) && uniqueName != col.HeaderText)
                            {
                                dt.Columns.Remove(col.HeaderText);
                            }
                            dt.Columns[uniqueName].ColumnName = col.HeaderText;

                            iColSeq++;
                        }
                        else if (dt.Columns.Contains(col.HeaderText))
                        {
                            dt.Columns[col.HeaderText].SetOrdinal(iColSeq);
                            dt.Columns[col.HeaderText].ColumnName = col.HeaderText;

                            iColSeq++;
                        }
                        string strDataFormatString = "", strAggregate = "", strFooterAggregateFormatString = "", strFooterText = "";
                        switch (col.ColumnType)
                        {
                            case "GridBoundColumn":
                                strDataFormatString = ((Telerik.Web.UI.GridBoundColumn)col).DataFormatString;
                                strAggregate = ((Telerik.Web.UI.GridBoundColumn)col).Aggregate.ToString();
                                strFooterAggregateFormatString = ((Telerik.Web.UI.GridBoundColumn)col).FooterAggregateFormatString;
                                strFooterText = ((Telerik.Web.UI.GridBoundColumn)col).FooterText;
                                break;

                            case "GridNumericColumn":
                                strDataFormatString = ((Telerik.Web.UI.GridNumericColumn)col).DataFormatString;
                                strAggregate = ((Telerik.Web.UI.GridNumericColumn)col).Aggregate.ToString();
                                strFooterAggregateFormatString = ((Telerik.Web.UI.GridNumericColumn)col).FooterAggregateFormatString;
                                strFooterText = ((Telerik.Web.UI.GridNumericColumn)col).FooterText;
                                break;
                        }
                        if (!String.IsNullOrEmpty(strDataFormatString) || (!String.IsNullOrEmpty(strAggregate) && strAggregate != "None") || !String.IsNullOrEmpty(strFooterAggregateFormatString) || !String.IsNullOrEmpty(strFooterText))
                        {
                            strAggregate = ConvertASPtoExcelFormat(strAggregate);
                            strDataFormatString = ConvertASPtoExcelFormat(strDataFormatString);
                            strFooterAggregateFormatString = ConvertASPtoExcelFormat(strFooterAggregateFormatString);
                            lstFormatedColumns.Add(new Tuple<String, String, String, String, String>(col.HeaderText, strAggregate, strDataFormatString, strFooterAggregateFormatString, strFooterText));
                        }
                    }
                }
                while (iColSeq != dt.Columns.Count)
                {
                    dt.Columns.RemoveAt(iColSeq);
                }

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds != null && ds.Tables[0] != null)
                {
                    EDM.Common.Log.Info(Module, "ExportToExcel", "## Step-8 : At end ExportToExcel function record count is = " + Convert.ToString(ds.Tables[0].Rows.Count) + ", FileName = " + sheetName);
                }

                Export(ds, gemBoxKey, sheetName, lstFormatedColumns, wrapText);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "ExportToExcel", ex);

                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                EDM.Common.Log.Error(Module, Module + ":EDM.DataExport", "ExportToExcel", message);
            }
        }


        public void ExportListToExcel(string gemBoxKey, DataSet dsExportData, string sheetName, bool wrapText = false)
        {
            try
            {
                string NavUrlPath = string.Empty; //HttpContext.Current.Request.Path;
                sheetName = GetPlainFileName(sheetName);

                //if (uxRadGrid == null || uxRadGrid.Items.Count == 0) return;
                if (dsExportData == null) return;
                if (dsExportData.Tables[sheetName] == null) return;
                int iColSeq = 0;
                DataTable dt = dsExportData.Tables[sheetName];
                //folowing list is as - ColumnName, Aggregate (ASP Format), DataFormatString  (ASP Format), FooterAggregateFormatString  (ASP Format), FooterText
                List<Tuple<String, String, String, String, String>> lstFormatedColumns = new List<Tuple<String, String, String, String, String>>();
                if (dt != null)
                {
                    EDM.Common.Log.Info(Module, "ExportToExcel", "## Step-7 : At start ExportToExcel function record count is = " + Convert.ToString(dt.Rows.Count) + ", FileName = " + sheetName);
                }

                //foreach (GridColumn col in uxRadGrid.Columns)
                //{
                //    if (col.Visible)
                //    {
                //        var uniqueName = col.UniqueName == "UserCompanyName" ? "CompanyName" : col.UniqueName;
                //        if (dt.Columns.Contains(uniqueName))
                //        //if (dt.Columns.Contains(col.UniqueName))
                //        {
                //            dt.Columns[uniqueName].SetOrdinal(iColSeq);
                //            if (dt.Columns.Contains(col.HeaderText) && uniqueName != col.HeaderText)
                //            {
                //                dt.Columns.Remove(col.HeaderText);
                //            }
                //            dt.Columns[uniqueName].ColumnName = col.HeaderText;

                //            iColSeq++;
                //        }
                //        else if (dt.Columns.Contains(col.HeaderText))
                //        {
                //            dt.Columns[col.HeaderText].SetOrdinal(iColSeq);
                //            dt.Columns[col.HeaderText].ColumnName = col.HeaderText;

                //            iColSeq++;
                //        }
                //        string strDataFormatString = "", strAggregate = "", strFooterAggregateFormatString = "", strFooterText = "";
                //        switch (col.ColumnType)
                //        {
                //            case "GridBoundColumn":
                //                strDataFormatString = ((Telerik.Web.UI.GridBoundColumn)col).DataFormatString;
                //                strAggregate = ((Telerik.Web.UI.GridBoundColumn)col).Aggregate.ToString();
                //                strFooterAggregateFormatString = ((Telerik.Web.UI.GridBoundColumn)col).FooterAggregateFormatString;
                //                strFooterText = ((Telerik.Web.UI.GridBoundColumn)col).FooterText;
                //                break;

                //            case "GridNumericColumn":
                //                strDataFormatString = ((Telerik.Web.UI.GridNumericColumn)col).DataFormatString;
                //                strAggregate = ((Telerik.Web.UI.GridNumericColumn)col).Aggregate.ToString();
                //                strFooterAggregateFormatString = ((Telerik.Web.UI.GridNumericColumn)col).FooterAggregateFormatString;
                //                strFooterText = ((Telerik.Web.UI.GridNumericColumn)col).FooterText;
                //                break;
                //        }
                //        if (!String.IsNullOrEmpty(strDataFormatString) || (!String.IsNullOrEmpty(strAggregate) && strAggregate != "None") || !String.IsNullOrEmpty(strFooterAggregateFormatString) || !String.IsNullOrEmpty(strFooterText))
                //        {
                //            strAggregate = ConvertASPtoExcelFormat(strAggregate);
                //            strDataFormatString = ConvertASPtoExcelFormat(strDataFormatString);
                //            strFooterAggregateFormatString = ConvertASPtoExcelFormat(strFooterAggregateFormatString);
                //            lstFormatedColumns.Add(new Tuple<String, String, String, String, String>(col.HeaderText, strAggregate, strDataFormatString, strFooterAggregateFormatString, strFooterText));
                //        }
                //    }
                //}
                //while (iColSeq != dt.Columns.Count)
                //{
                //    dt.Columns.RemoveAt(iColSeq);
                //}

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds != null && ds.Tables[0] != null)
                {
                    EDM.Common.Log.Info(Module, "ExportToExcel", "## Step-8 : At end ExportToExcel function record count is = " + Convert.ToString(ds.Tables[0].Rows.Count) + ", FileName = " + sheetName);
                }

                Export(ds, gemBoxKey, sheetName, lstFormatedColumns, wrapText);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "ExportToExcel", ex);

                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                EDM.Common.Log.Error(Module, Module + ":EDM.DataExport", "ExportToExcel", message);
            }
        }



        public DataTable GetFilteredDataSource(RadGrid uxRadGrid, string tableName)
        {
            try
            {
                tableName = GetPlainFileName(tableName);
                if (uxRadGrid == null || uxRadGrid.Items.Count == 0) return null;
                if (uxRadGrid.DataSource == null) return null;

                DataTable dt = new DataTable();
                DataTable FilteredDt = new DataTable();

                string filterexpression = string.Empty;
                filterexpression = uxRadGrid.MasterTableView.FilterExpression;

                int sortCnt = uxRadGrid.MasterTableView.SortExpressions.Count;
                string sort = "";
                if (sortCnt > 0)
                {
                    sort = uxRadGrid.MasterTableView.SortExpressions[0].ToString();
                }

                dt = (DataTable)uxRadGrid.DataSource;
                if (String.IsNullOrEmpty(filterexpression))
                {
                    FilteredDt = dt;
                }
                else
                {
                    FilteredDt = dt.AsEnumerable()
                    .AsQueryable()
                    .Where(filterexpression)
                    .CopyToDataTable();
                }

                DataView dv = FilteredDt.DefaultView;
                dv.Sort = sort;
                DataTable SortedDt = dv.ToTable(tableName);
                return SortedDt;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "GetFilteredDataSource", ex);
                return null;
            }
        }
        public DataTable GetFilteredData(DataTable dt, string filterexpression, string sort, string tableName)
        {
            try
            {
                tableName = GetPlainFileName(tableName);
                DataTable FilteredDt = new DataTable();

                if (String.IsNullOrEmpty(filterexpression))
                {
                    FilteredDt = dt;
                }
                else
                {
                    FilteredDt = dt.AsEnumerable()
                    .AsQueryable()
                    .Where(filterexpression)
                    .CopyToDataTable();
                }

                DataView dv = FilteredDt.DefaultView;
                dv.Sort = sort;
                DataTable SortedDt = dv.ToTable(tableName);
                return SortedDt;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "GetFilteredDataSource", ex);

                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                Common.Log.Error(Module, Module + ":EDM.DataExport", "GetFilteredDataSource", message);

                return null;
            }
        }
        public DataSet SetFilteredDataSource(DataSet _DsExportData, DataTable dt, string sheetName)
        {
            try
            {
                string strSheetName = GetPlainFileName(sheetName);
                if (_DsExportData.Tables[strSheetName] == null)
                {
                    _DsExportData.Tables.Add(dt);
                }
                else
                {
                    _DsExportData.Tables.Remove(strSheetName);
                    _DsExportData.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "SetFilteredDataSource", ex);

                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                EDM.Common.Log.Error(Module, Module + ":EDM.DataExport", "SetFilteredDataSource", message);
            }
            return _DsExportData;
        }

        //folowing lstFormatedColumns is as - ColumnName, Aggregate (Excel Format), DataFormatString (Excel Format), FooterAggregateFormatString (Excel Format), FooterText
        public void Export(DataSet ds, string gemBoxKey, string fileName, List<Tuple<String, String, String, String, String>> lstFormatedColumns, bool wrapText = false)
        {
            try
            {
                var contentRootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
                var path = "\\Tools\\CustomerCommunications\\ApplicationDoc\\";
                string NavUrlPath = contentRootPath + path; // HttpContext.Current.Request.Path;

                fileName = GetPlainFileName(fileName);
                GemBoxKey = gemBoxKey;
                SpreadsheetInfo.SetLicense(GemBoxKey);

                ExcelFile ef = new ExcelFile();

                // Imports all tables from DataSet to new file.
                foreach (DataTable dataTable in ds.Tables)
                {

                    EDM.Common.Log.Info(Module, "Export", "## Step-9 : At start Export function record count is = " + dataTable.Rows.Count + ", FileName = " + fileName);

                    string worksheetName = "Export";
                    if (dataTable.TableName.Length > 30)
                        worksheetName = dataTable.TableName.Substring(0, 30);
                    else
                        worksheetName = dataTable.TableName;
                    ExcelWorksheet ws = ef.Worksheets.Add(worksheetName);

                    ws.InsertDataTable(dataTable, new InsertDataTableOptions(0, 0) { ColumnHeaders = true });
                    ws.Panes = new WorksheetPanes(PanesState.Frozen, 0, 1, "A2", PanePosition.BottomLeft);
                    int rowCount = ws.Rows.Count;

                    EDM.Common.Log.Info(Module, "Export", "## Row count after add header = " + rowCount + ", FileName = " + fileName);


                    #region Column Formatting

                    int ColumnCount = ws.CalculateMaxUsedColumns();
                    for (int i = 0; i < ColumnCount; i++)
                    {
                        //Code for Date format
                        if (dataTable.Columns[i].DataType == typeof(DateTime) && !lstFormatedColumns.Any(t => t.Item1 == dataTable.Columns[i].ColumnName))
                        {
                            DataRow row = dataTable.Select("[" + dataTable.Columns[i].ColumnName + "] is not null AND  [" + dataTable.Columns[i].ColumnName + "]  <> '1900-01-01'").LastOrDefault();
                            string strDate = "";
                            if (row != null)
                            {
                                strDate = Convert.ToDateTime(row[dataTable.Columns[i]]).ToString("hh:mm:ss tt");
                            }

                            if (strDate == "12:00:00 AM")
                            {
                                ws.Columns[i].Style.NumberFormat = shortDateFormat;
                            }
                            else
                            {
                                ws.Columns[i].Style.NumberFormat = dateFormat;
                            }
                        }
                        //Code for Numberformat with different decimal points
                        if ((dataTable.Columns[i].DataType == typeof(Decimal) || dataTable.Columns[i].DataType == typeof(Double)))
                        {
                            string strNumberFormat = "###0.";
                            int count = 0;
                            if (dataTable.Rows.Count > 0)
                            {
                                int len = dataTable.AsEnumerable().Select(row => row[i].ToString()).Max(val => val.Length);
                                string strValue = dataTable.AsEnumerable().Select(row => row[i].ToString()).Where(r => r.Length == len).FirstOrDefault();
                                // strValue = Convert.ToString(dataTable.Rows[0][i]);
                                count = strValue.Length - strValue.IndexOf('.');
                            }

                            for (int icnt = 1; icnt < count; icnt++)
                            {
                                strNumberFormat = strNumberFormat + "0";
                            }
                            if (count > 1)
                            {
                                ws.Columns[i].Style.NumberFormat = strNumberFormat;
                            }
                            else
                            {
                                ws.Columns[i].Style.NumberFormat = "@";
                            }
                        }
                        //Code for Amount sum Avg or column format
                        if (lstFormatedColumns.Any(t => t.Item1 == dataTable.Columns[i].ColumnName))
                        {
                            Tuple<String, String, String, String, String> tplItem = lstFormatedColumns.FirstOrDefault(t => t.Item1 == dataTable.Columns[i].ColumnName);
                            if (!String.IsNullOrEmpty(tplItem.Item2) && rowCount > 1)
                            {
                                string namedRange = "Range" + i;
                                string firstCell = ws.Columns[i].Cells[1].Name;
                                string lastCell = ws.Columns[i].Cells[rowCount - 1].Name;

                                ws.NamedRanges.Add(namedRange, ws.Cells.GetSubrange(firstCell, lastCell));

                                ws.Columns[i].Cells[rowCount].Formula = "=" + tplItem.Item2 + "(" + namedRange + ")";
                            }
                            if (!String.IsNullOrEmpty(tplItem.Item3))
                            {
                                ws.Columns[i].Style.NumberFormat = tplItem.Item3;
                            }
                            if (!String.IsNullOrEmpty(tplItem.Item4))
                            {
                                ws.Columns[i].Cells[rowCount].Style.NumberFormat = tplItem.Item4;
                            }
                            if (!String.IsNullOrEmpty(tplItem.Item5))
                            {
                                ws.Columns[i].Cells[rowCount].Value = tplItem.Item5;
                            }
                        }

                        ws.Columns[i].SetWidth(5500, LengthUnit.ZeroCharacterWidth256thPart);

                        if (wrapText)
                            ws.Columns[i].Style.WrapText = true;

                        ws.Columns[i].Cells[0].Style.Font.Size = 14 * 20;
                        ws.Columns[i].Cells[0].Style.Font.Weight = ExcelFont.BoldWeight;
                        ws.Columns[i].Cells[0].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Background1Darker15Pct), SpreadsheetColor.FromName(ColorName.Black));
                    }

                    #endregion Column Formatting

                    EDM.Common.Log.Info(Module, "Export", "## Step-10 : At end Export function record count is = " + Convert.ToString(dataTable.Rows.Count) + ", FileName = " + fileName);
                }
                string strDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string strURL = NavUrlPath + fileName + strDateTime + ".xlsx"; // HttpContext.Current.Server.MapPath(fileName + strDateTime + ".xlsx");
                ef.Save(strURL);

                FilePath = strURL;
                //Downloadfile(fileName, strURL);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "Export", ex);

                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                EDM.Common.Log.Error(Module, Module + ":EDM.DataExport", "Export", message);
            }
        }

        public void SetViewState(RadGrid uxRadGrid, out string filterexpression, out string sort)
        {
            filterexpression = String.Empty;
            sort = String.Empty;
            try
            {
                if (uxRadGrid != null && uxRadGrid.Items.Count != 0 && uxRadGrid.DataSource != null)
                {
                    filterexpression = uxRadGrid.MasterTableView.FilterExpression;

                    int sortCnt = uxRadGrid.MasterTableView.SortExpressions.Count;

                    if (sortCnt > 0)
                    {
                        sort = uxRadGrid.MasterTableView.SortExpressions[0].ToString();
                    }

                }
            }
            catch (Exception ex) { Common.Log.Error(Module, Module + ":EDM.DataExport", "SetViewState", ex); }
        }

        public void SetViewState4MultipleGrids(RadGrid uxRadGrid, out string filterexpression, out string sort)
        {
            filterexpression = String.Empty;
            sort = String.Empty;
            try
            {
                filterexpression = uxRadGrid.MasterTableView.FilterExpression;

                int sortCnt = uxRadGrid.MasterTableView.SortExpressions.Count;

                if (sortCnt > 0)
                {
                    sort = uxRadGrid.MasterTableView.SortExpressions[0].ToString();
                }
            }

            catch (Exception ex) { Common.Log.Error(Module, Module + ":EDM.DataExport", "SetViewState4MultipleGrids", ex); }

        }

        #endregion --- Public Methods ---

        #region --- Private Methods ---

        //public IActionResult DownloadFile()
        //{
        //    // Set the file path and content type
        //    var filePath = "/path/to/your/file.pdf";
        //    var contentType = "application/pdf";

        //    // Return the file to the client as a download
        //    var fileContent = System.IO.File.ReadAllBytes(filePath);
        //    return File(fileContent, contentType, Path.GetFileName(filePath));
        //}

        private void Downloadfile(string sFileName, string sFilePath)
        {
            try
            {

                //var contentType = "application/octet-stream";

                //// Return the file to the client as a download
                //var fileContent = System.IO.File.ReadAllBytes(sFilePath);
                //return File(fileContent, contentType, Path.GetFileName(sFilePath));


                //var file = new System.IO.FileInfo(sFilePath);

                //var response = HttpContext.Response;
                //response.Clear();
                //response.ContentType = "application/octet-stream";
                //response.Headers.Add("content-disposition", "attachment; filename=" + sFileName + ".xlsx");
                //response.Headers.Add("Content-Length", file.Length.ToString(System.Globalization.CultureInfo.InvariantCulture));
                //response.WriteFile(file.FullName);
                //response.Flush();
                //response.StatusCode = (int)HttpStatusCode.OK;
                //response.ContentType = "application/octet-stream";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "Downloadfile", ex);
            }
        }



        //private void Downloadfile(string sFileName, string sFilePath)
        //{
        //    try
        //    {
        //        var file = new System.IO.FileInfo(sFilePath);
        //        HttpContext.Current.Response.Clear();
        //        HttpContext.Current.Response.ContentType = "application/octet-stream";
        //        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName + ".xlsx");
        //        HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString(System.Globalization.CultureInfo.InvariantCulture));
        //        HttpContext.Current.Response.WriteFile(file.FullName);
        //        HttpContext.Current.Response.Flush();
        //        // Prevents any other content from being sent to the browser
        //        HttpContext.Current.Response.SuppressContent = true;
        //        HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        file.Delete();
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = ex.Message;
        //        Common.Log.Error(Module, Module + ":EDM.DataExport", "Downloadfile", ex);
        //    }
        //}

        public string GetPlainFileName(string fileName)
        {
            try
            {
                fileName = fileName.Replace(": ", "_").Replace(" ", "_").Replace("-", "");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "GetPlainName", ex);
            }
            return fileName;
        }

        private string ConvertASPtoExcelFormat(string strColFormat)
        {
            if (strColFormat.Length == 0) return "";
            string strResult = "", strOriginal = strColFormat;
            try
            {
                if (strColFormat.IndexOf("{") > -1)
                {
                    strColFormat = strColFormat.Substring(strColFormat.IndexOf("{"), strColFormat.IndexOf("}") - strColFormat.IndexOf("{") + 1);
                }
                switch (strColFormat)
                {
                    case "Sum":
                        strResult = "SUM";
                        break;
                    case "Avg":
                        strResult = "AVERAGE";
                        break;
                    case "{0:F0}":
                    case "{0}":
                    case "{0:0}":
                        strResult = "0";
                        break;
                    case "{0:F}":
                        strResult = "0.00";
                        break;
                    case "{0:C}":
                    case "{0:$ #,##0.00}":
                        strResult = "$ #,##0.00_);($ #,##0.00)";
                        break;
                    case "{0:MM/dd/yyyy}":
                        strResult = "mm/dd/yyyy";
                        break;
                    case "{0:MM/dd/yyyy hh:mm:ss tt}":
                        strResult = "mm/dd/yyyy HH:MM:ss AM/PM";
                        break;
                    case "{0:MM/dd/yyyy hh:mm tt}":
                        strResult = "mm/dd/yyyy HH:MM AM/PM";
                        break;
                    case "{0:MM/dd/yy}":
                        strResult = "mm/dd/yy";
                        break;
                    case "None":
                        strResult = "";
                        break;
                    case "{0:$#,##0.00;$-#,##0.00}":
                        strResult = "$#,##0.00;$-#,##0.00";
                        break;
                    default:
                        Common.Log.Info(Module, Module + ":EDM.DataExport:ConvertASPtoExcelFormat", "ExcelFormat not found for " + strColFormat);
                        break;
                }
                strResult = strOriginal.Replace(strColFormat, strResult);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.DataExport", "ConvertASPtoExcelFormat", ex);
            }
            return strResult;
        }
        #endregion --- Private Methods ---
    }
}