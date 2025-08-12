using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using test01.Authentication;
using test01.Repositoies;

namespace test01.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(JwtOptions jwtOptions,UsersRepo usersRepo) : ControllerBase
    {
        [HttpPost]
        [Route("auth")]

        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            if (request is null)
            {
                return BadRequest("Invalid request");
            }
            var user = usersRepo.GetUserByNameAsync(request.Username).Result;
            if (user is null|| user.Password!=request.Password)
            {
                return Unauthorized("Wrong usename or password ");
            }

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
                    new (ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new (ClaimTypes.Name, user.Name),
                })
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            return Ok(accessToken);

        }
    }
}
