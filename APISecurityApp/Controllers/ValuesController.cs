using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APISecurityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "MostBeAdmin")]
        public ActionResult<string> Get()
        {
            return Ok("This Is a Get Method");
        }
    }
}
