using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace test01.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IOptions<AttachmentsOptions> attachments;

        public ConfigController(IConfiguration configuration, IOptions<AttachmentsOptions> attachments)
        {
            this.configuration = configuration;
            this.attachments = attachments;
        }
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetConfig()
        {
            var ConfigValues = new
            {
                AllowedHosts = configuration["AllowedHosts"],
                //ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                EnvironmentName = configuration["ASPNETCORE_ENVIRONMENT"],
                ConnectionString = configuration.GetConnectionString("DefaultConnection"),
                LoggingLevelDefault = configuration["Logging:LogLevel:Default"],
                LoggingLevelMicrosoft = configuration.GetSection("Logging").GetSection("LogLevel")["Microsoft.AspNetCore"],
                TaskKey = configuration["TestKey"],
                SigninKey = configuration["SigninKey"],
                AttachmentsOptions = attachments.Value, 
            };
            return Ok(ConfigValues);
        }
    }
}
