using EDMS.DSM.Server.Models;
using EDMS.DSM.Shared.Models;
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
    }
}
