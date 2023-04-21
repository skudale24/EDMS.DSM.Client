using EDMS.DSM.Server.Models;
using EDMS.DSM.Shared.Models;
using EDMS.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDMS.DSM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<BooksController>
        [HttpGet("List")]
        public async Task<IApiResult> GetCommunications()
        {
            var communications = _context.Database.SqlQuery<Communications>($"EXECUTE dbo.[p_Get_HUP_AggregateList4CustomerCommunications_1]").ToList();
            
			return ApiResult<List<Communications>>.Success(communications);
        }
    }
}
