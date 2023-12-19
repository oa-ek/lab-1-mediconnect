using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetConfig()
        {
            var configData = new
            {
                DatabaseConnectionString = _configuration.GetConnectionString("DefaultConnection"),
                SomeOtherConfigValue = _configuration["SomeOtherConfigKey"]
            };

            return Ok(configData);
        }
    }
}
