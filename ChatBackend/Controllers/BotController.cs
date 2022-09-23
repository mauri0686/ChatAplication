using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public BotController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet(Name = "stock={stock_code}")]
        public async Task Get(string stock_code)
        {
            
        }
    }
}