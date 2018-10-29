using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Handallo.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Handallo.Controllers
{

   
    [Route("api/[controller]")]
    [ApiController]
    public class AdministerController : ControllerBase
    {
        public readonly AdministerDataProvider __AdministerDataProvider;
        private IConfiguration _config;
        UserModel result;
        public AdministerController(IConfiguration config)
        {
            __AdministerDataProvider = new AdministerDataProvider();
            _config = config;
        }
        // GET: api/Adminster
        /* [HttpGet]
         public IEnumerable<string> Get()
         {
             return new string[] { "value1", "value2" };
         }*/

        // GET: api/Adminster/5
        [HttpGet("{id}")]
        public Administer Get(int id)
        {
            return __AdministerDataProvider.GetAdminister(id);
        }



        // POST: api/Adminster
        [HttpPost("register")]
        public ActionResult Post([FromBody] Administer administer)
        {
            if (__AdministerDataProvider.RegisterAdmin(administer))
            {
                return Ok("register sucessfully");
            }

            return BadRequest("user already exists");
        }

        [HttpPost("login")]
        public IActionResult Post([FromBody] Login login)
        {
            result = __AdministerDataProvider.LoginAdmin(login);
            if (result == null)
            {
                return new BadRequestResult();
            }

            String token = (BuildToken(result));
            return new OkObjectResult(new { token = token });
        }

        private string BuildToken(UserModel user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // PUT: api/Adminster/5
        /* [HttpPut("{id}")]
         public void Put(int id, [FromBody] string value)
         {
         }

         // DELETE: api/ApiWithActions/5
         [HttpDelete("{id}")]
         public void Delete(int id)
         {
         }*/
    }
}
