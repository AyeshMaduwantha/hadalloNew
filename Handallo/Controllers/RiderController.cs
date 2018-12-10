using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Handallo.DataProvider;
using Handallo.DataProvider.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Handallo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiderController : ControllerBase
    {
        public readonly RiderDataProvider _RiderDataProvider;
        private IConfiguration _config;
        public readonly ShopDataProvider _ShopDataProvider;
        public readonly OrderDataProvider _OrderDataProvider;

        public RiderController(IConfiguration config)
        {
            
            _RiderDataProvider = new RiderDataProvider();
            _config = config;
        }

        // GET: api/Rider
       /* [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Rider/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/Rider
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromForm] Rider rider)
        {
            return await _RiderDataProvider.RegisterRider(rider);
    ;
        }

        [HttpPost("login")]
        public IActionResult Post([FromBody] Login login)
        {
           UserModel result = _RiderDataProvider.LoginRider(login);
            if (result == null)
            {
                return new ConflictResult();
            }
            String token = (BuildToken(result));
            return new OkObjectResult(new { token = token });
        }

        [HttpGet]
        [Route("download/{id:int}")]
        public Task<IActionResult> Getfiles(int id)
        {

            String path = _RiderDataProvider.DownloadImage(id);
            return Download(path);

        }

        [HttpGet]
        [HttpGet("vieworders")]
        public IActionResult Vieworders()
        {
            return new JsonResult(_OrderDataProvider.ViewApprovedOrders());
        }






        // PUT: api/Rider/5
        /* [HttpPut("{id}")]
         public void Put(int id, [FromBody] string value)
         {
         }

         // DELETE: api/ApiWithActions/5
         [HttpDelete("{id}")]
         public void Delete(int id)
         {
         }*/

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

        public async Task<IActionResult> Download(string path)
        {

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".jfif","image/jfif" },
                {".csv", "text/csv"}
            };
        }
    }
}
