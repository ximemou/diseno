using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportManagement.Data;
using SportManagement.WebApi.Services;
using Microsoft.Extensions.Configuration;
using SportManagement.WebApi.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportManagement.WebApi.Controllers
{
    [Route("api/logIn")]
    public class LogInController : Controller
    {
        private IConfiguration config;
        private ILogInService logInService;

        public LogInController(IConfiguration config,ILogInService service)
        {
            this.config = config;
            this.logInService = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LogInModel login)
        {
            
            IActionResult response = Unauthorized();
            User user = logInService.Authenticate(login.Username,login.Password);
            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            string role = "";
            if (user.IsAdministrator)
            {
            
                role = "Administrator";

            }
            else
            {
                role = "User";
            }


            var claims = new List<Claim>
             {
              new Claim(ClaimTypes.Name, user.UserName),
              new Claim(ClaimTypes.Role, role)
              };

                var token = new JwtSecurityToken(
                 issuer:   config["Jwt:Issuer"],
           audience:config["Jwt:Issuer"],
           claims:claims,
           expires: DateTime.Now.AddMinutes(30),
           signingCredentials: creds);
                return new JwtSecurityTokenHandler().WriteToken(token);
              
        }
    }
}

