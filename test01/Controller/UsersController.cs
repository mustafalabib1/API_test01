using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using test01.Authentication;

namespace test01.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(JwtOptions jwtOptions) : ControllerBase
    {
        [HttpPost]
        [Route("auth")]

        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Signinkey)),
                    algorithm: SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.LifeTime),
                Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new (ClaimTypes.NameIdentifier,request.Username),
                    new (ClaimTypes.Email, "abc@Gmail.com"),
                })
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            return Ok(accessToken);

        }
    }
}
