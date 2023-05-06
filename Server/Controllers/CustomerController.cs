//using EDM.DataExport;
using EDM.PDFMappingVariables;
using EDM.Setting;
using EDMS.DSM.Server.Models;
using EDMS.DSM.Shared.Models;
using EDMS.Shared.Enums;
using EDMS.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Data;
using System.IO;
using System.Net;
//using VTI.Common;

namespace EDMS.DSM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/<BooksController>
        [HttpGet("List/{programId}")]
        public async Task<IApiResult> GetCommunications(int programId)
        {
            var communications = _context.Database.SqlQueryRaw<Communications>($"EXECUTE dbo.[p_Get_HUP_AggregateList4CustomerCommunications]").ToList();
            return ApiResult<List<Communications>>.Success(communications);
        }

        //[HttpGet("downloadexcelfile/{FileName}")]
        //public async Task DownloadExcelFile([FromBody] LetterDetails letterDetails)
        //{
        //    try
        //    {
        //        //args = e.CommandArgument.ToString().Split(',');
        //        DataSet dsExport = new DataSet();
        //        DataTable dtExport = new DataTable();
        //        if (letterDetails.TemplateFile.Equals("Generate Letters", StringComparison.OrdinalIgnoreCase))
        //        {
        //            EDM.PDFMappingVariables.PDFGeneration objForExcel = new EDM.PDFMappingVariables.PDFGeneration();
        //            objForExcel.ProgramId = letterDetails.ProgramId;
        //            objForExcel.TemplateID = letterDetails.TemplateID;
        //            objForExcel.LPCID = letterDetails.LPCID;
        //            objForExcel.TemplateType = letterDetails.TemplateType;
        //            dsExport = objForExcel.GetData();
        //            //if (!MsSql.IsEmpty(dsExport))
        //            //{
        //            dtExport.Columns.Add("Application ID");
        //            dtExport.Columns.Add("Applicant Name");
        //            dtExport.Columns.Add("Letter Type");
        //            dtExport.Columns.Add("LPC Name");
        //            dtExport.Columns.Add("Date Generated");

        //            foreach (DataRow dr in dsExport.Tables[0].Rows)
        //            {
        //                DataRow drExport = dtExport.NewRow();
        //                drExport["Application ID"] = dr["ApplicationId"];
        //                drExport["Applicant Name"] = dr["PrimaryApplicantName"].ToString().TrimEnd(',');
        //                //drExport["Letter Type"] = args[6];
        //                //drExport["LPC Name"] = args[5];
        //                drExport["Date Generated"] = string.Empty;

        //                dtExport.Rows.Add(drExport);
        //            }
        //            dsExport.Tables.Remove(dsExport.Tables[0]);
        //            dsExport.Tables.Add(dtExport);
        //            //}

        //        }
        //        else
        //        {
        //            EDM.CommunicationTemplate.CustomerCommunications objComTran = new EDM.CommunicationTemplate.CustomerCommunications();
        //            //dsExport = objComTran.GetApplicationDataByBatchId(Int32.Parse(args[4]));
        //        }

        //        //if (!MsSql.IsEmpty(dsExport))
        //        //{
        //        //    dsExport.Tables[0].TableName = "CustomerList";
        //        //    ExcelExport objExport = new ExcelExport(EDM.Setting.Module.AdminPortal);
        //        //    //objExport.Export(dsExport, "", "CustomerList_" + args[6] + "_" + (args[5] + "_" + DateTime.Now.ToString("MMddyy")), new List<Tuple<String, String, String, String, String>>());
        //        //}

        //        //string? uploadFilePath = _configuration["UploadFilePath"];
        //        //var decodedFileName = WebUtility.UrlDecode(FileName).Replace("/", "\\");
        //        //string filePath = Path.Join(uploadFilePath, decodedFileName);
        //        //filePath = filePath.Replace("\\\\", "\\");
        //        //var l_sReader = System.IO.File.OpenRead(filePath);
        //        //return (File(l_sReader, "application/octet-stream", Path.GetFileName(filePath)));
        //    }
        //    catch (Exception ex)
        //    {
        //        //_logger.LogError(ex.Message);
        //        //return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
        //    }
        //}

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
                //_logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }

        [HttpPut("generateletter")]
        public async Task<IActionResult> GenerateLetter([FromBody] LetterDetails letterDetails)
        {
            try
            {

                PDFGeneration obj = new PDFGeneration();
                obj.ConnectionString = _configuration.GetConnectionString("Default");
                obj.TemplateFile = letterDetails.TemplateFile;

                //NewLocalPath
                //obj.NewLocalPath = _configuration["UploadFilePath"];
                var contentRootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
                var path = "\\Tools\\CustomerCommunications\\ApplicationDoc\\";
                obj.NewLocalPath = contentRootPath + path;

                obj.ProgramId = letterDetails.ProgramId;
                obj.TemplateID = letterDetails.TemplateID;
                obj.LPCID = letterDetails.LPCID;
                obj.TemplateType = letterDetails.TemplateType;
                obj.GeneratedBy = letterDetails.GeneratedBy;
                obj.GetTemplatePDFGeneration();
                var GeneratedPath = obj.GeneratedFilePath;
                var BatchId = obj.BatchId;
                string letterType = Enum.GetName(typeof(ETemplateType), letterDetails.TemplateType);
                var OriginalName = DateTime.Now.ToString("yyyyMMdd") + "_CC" + letterType + ".pdf";

                return Ok(await ApiResult<PDFGeneration>.SuccessAsync(obj));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }
    }
}
