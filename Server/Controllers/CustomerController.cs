using EDM.PDFMappingVariables;
using EDM.Setting;
using EDMS.DSM.Server.Models;
using EDMS.DSM.Shared.Models;
using EDMS.Shared.Enums;
using EDMS.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;

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
        [HttpGet("List")]
        public async Task<IApiResult> GetCommunications()
        {
            var communications = _context.Database.SqlQueryRaw<Communications>($"EXECUTE dbo.[p_Get_HUP_AggregateList4CustomerCommunications]").ToList();

            return ApiResult<List<Communications>>.Success(communications);
        }

        [HttpGet("downloadsourcefile/{FileName}")]
        public async Task<IActionResult> DownloadSourceFile(string FileName)
        {
            try
            {
                string? uploadFilePath = _configuration["UploadFilePath"];
                /*
				string fileNameWithOutExt = Path.GetFileNameWithoutExtension(CSVFileName);
				string fileExtension = Path.GetExtension(CSVFileName);

				if (fileExtension != ".csv")
				{
					return BadRequest(ApiResult.Fail("Only .csv file errors can be downloaded."));
				}

				string generatedFileName = $"{fileNameWithOutExt}_err.txt";
				*/

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
