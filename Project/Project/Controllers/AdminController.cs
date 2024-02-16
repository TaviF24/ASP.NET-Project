using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {



        [HttpGet("employees"),Authorize(Roles = "Admin")]
        public IEnumerable<string> Get()
        {
            return new List<string> { "User1", "User2", "User3"};
        }



    }
}
