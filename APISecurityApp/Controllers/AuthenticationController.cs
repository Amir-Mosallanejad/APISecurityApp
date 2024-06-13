using APISecurityApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APISecurityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthenticationController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpPost("token")]
        public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
        {
            var user = ValidateUser(data);

            if (data == null)
            {
                return Unauthorized();
            }
            var token = generateToken(user);

            return Ok(token);

        }
        private string generateToken(Users user)
        {
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));

            var signingCridentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new();
            claims.Add(new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()));
            claims.Add(new(JwtRegisteredClaimNames.Name, user.UserName));
            if (user.IsAdmin == true)
            {
                claims.Add(new("IsAdmin", "True"));
            }
            else { claims.Add(new("IsAdmin", "False")); }
            var token = new JwtSecurityToken(
                _config.GetValue<string>("Authentication:Issuer"),
               _config.GetValue<string>("Authentication:Audience"),
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(1),
                signingCridentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Users? ValidateUser(AuthenticationData data)
        {
            if (Validator(data.UserName, "Amir") &&
              Validator(data.Password, "123"))
            {
                return new Users() { UserId = 1, UserName = data.UserName, IsAdmin = true };
            }
            if (Validator(data.UserName, "mmd") &&
              Validator(data.Password, "123"))
            {
                return new Users() { UserId = 2, UserName = data.UserName };
            }

            return null;
        }

        private bool Validator(string actual, string expect)
        {
            if (actual is not null)
            {
                if (actual.Equals(expect))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
