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
    public class ShopOwnerController : ControllerBase
    {
        public readonly ShopOwnerDataProvider _ShopOwnerDataProvider;
        private IConfiguration _config;
        UserModel result;


        public ShopOwnerController(IConfiguration config)
        {
            _ShopOwnerDataProvider = new ShopOwnerDataProvider();
            _config = config;
        }
     /*   // GET: api/ShopOwner
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ShopOwner/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }*/

        [HttpPost("register")]
        public IActionResult Post([FromBody] ShopOwner shopowner)
        {
            result = _ShopOwnerDataProvider.RegisterShopOwner(shopowner);
            if (result == null)
            {
                return new BadRequestResult();
            }

            String token = (BuildToken(result));
            return new OkObjectResult(new { token = token });
        }

        [HttpPost("login")]
        public IActionResult Post([FromBody] Login login)
        {

             result = _ShopOwnerDataProvider.LoginShopOwner(login);
            if (result == null)
            {
                return new BadRequestResult();
            }

            String token = (BuildToken(result));
            return new OkObjectResult(new { token = token });


        }

        /*    // PUT: api/ShopOwner/5
            [HttpPut("{id}")]
            public void Put(int id, [FromBody] string value)
            {
            }

            // DELETE: api/ApiWithActions/5
            [HttpDelete("{id}")]
            public void Delete(int id)
            {
            } */

        private string BuildToken(UserModel user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
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
    }
}
