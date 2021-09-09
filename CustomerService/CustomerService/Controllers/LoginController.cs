using CustomerService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController:ControllerBase
    {
        private readonly IList<Credential> appUsers = new List<Credential>
        {
            new Credential {FullName="Admin User", UserName ="admin",Password="1234",UserRole="Admin"},
            new Credential {FullName="Test User", UserName ="user",Password="1234",UserRole="User"}
        };
        private readonly IConfiguration configuration;
        public LoginController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Credential credential)
        {
            var responseValue = this.authenticateuser(credential);
            if (responseValue!=null)
            {

                var securityKey = "SomeSupperSecretKey_AIzaSyClzfrOzB818x55FASHvX4JuGQciR9lv7q";
                const string JWT_ISSUER = "[https://localhost:51349/]https://localhost:51349/";
                const string JWT_AUDIENCE = "[https://localhost:51349/]https://localhost:51349/";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,responseValue.UserName),
                    new Claim("fullName",responseValue.FullName),
                    new Claim("role",responseValue.UserRole),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: JWT_ISSUER,
                    audience: JWT_AUDIENCE,
                    claims:claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var response = new ResponseCredential();
                response.UserName = responseValue.UserName;
                response.UserRole = responseValue.UserRole;

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userDetails = response
                });
            }

            return base.Unauthorized();
        }

        private Credential authenticateuser(Credential logincredentials)
        {
            Credential user = this.appUsers.SingleOrDefault(x =>
               x.UserName == logincredentials.UserName && x.Password == logincredentials.Password);
            return user;
        }
    }
}
