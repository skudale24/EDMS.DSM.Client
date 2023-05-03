using EDM.PDFMappingVariables;
using EDM.Setting;
using EDMS.DSM.Server.Models;
using EDMS.DSM.Shared.Models;
using EDMS.Shared.Enums;
using EDMS.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                string filePath = Path.Combine(uploadFilePath, FileName);

                var l_sReader = System.IO.File.OpenRead(filePath);
                return (File(l_sReader, "application/octet-stream", Path.GetFileName(FileName)));
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
                obj.NewLocalPath = _configuration["UploadFilePath"];
                //obj.NewLocalPath = Server.MapPath("~/Tools/CustomerCommunications/ApplicationDoc/");
                obj.ProgramId = 2;
                obj.TemplateID = letterDetails.TemplateID;
                obj.LPCID = letterDetails.LPCID;
                obj.TemplateType = letterDetails.TemplateType;
                obj.GeneratedBy = 10572;
                obj.GetTemplatePDFGeneration();
                var GeneratedPath = obj.GeneratedFilePath;
                var BatchId = obj.BatchId;
                string letterType = Enum.GetName(typeof(ETemplateType), letterDetails.TemplateType);
                var OriginalName = DateTime.Now.ToString("yyyyMMdd") + "_CC" + letterType + ".pdf";
                //this.ClientScript.RegisterStartupScript(Page.GetType(), "myScript", "ShowFile(" + "'Local','" + OriginalName + "','" + GeneratedPath + "');", true);

                //string? uploadFilePath = _configuration["UploadFilePath"];
                //FileInfo fileInfo = new(file.FileName);

                //string fileNameWithOutExt = Path.GetFileNameWithoutExtension(file.FileName);
                //string fileExtension = Path.GetExtension(file.FileName);

                //if (fileExtension != ".csv")
                //{
                //    return BadRequest(ApiResult.Fail("Only .csv file can be uploaded."));
                //}

                //string generatedFileName = $"{fileNameWithOutExt}_{DateTime.UtcNow.ToString(_uploadFileDateTimeFormat)}{fileExtension}";

                //string filePath = Path.Combine(uploadFilePath, generatedFileName);

                //_ = Directory.CreateDirectory(uploadFilePath);

                //string UserNameEmail = string.Empty;

                //Auth.Models.User? loggeduser = _usercacheSevice.GetUserByAspNetUserId(AspnetUserId).Result;
                //if (loggeduser != null)
                //{
                //    UserNameEmail = loggeduser.emailaddress;
                //}

                //using (FileStream stream = System.IO.File.Create(filePath))
                //{
                //    await file.CopyToAsync(stream);
                //    PricingUploadsDto pricing = new()
                //    {
                //        Status = PricingUploadFileStatus.New.ToString(),
                //        ActualFileName = file.FileName,
                //        FileType = fileType,
                //        UploadedDate = DateTime.UtcNow,
                //        orgId = orgId,
                //        UploadedBy = UserNameEmail,
                //        FileName = generatedFileName
                //    };
                //    await _pricingService.AddUploadAsync(pricing);
                //}

                return Ok(await ApiResult.SuccessAsync());
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ApiResult.Fail(ex.Message));
            }
        }
    }
}
