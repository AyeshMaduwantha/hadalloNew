using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handallo.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.Controllers
{

   
    [Route("api/[controller]")]
    [ApiController]
    public class AdministerController : ControllerBase
    {
        public readonly AdministerDataProvider __AdministerDataProvider;
        public AdministerController()
        {
            __AdministerDataProvider = new AdministerDataProvider();
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
        public Boolean Post([FromBody] Login login)
        {
            if (__AdministerDataProvider.LoginAdmin(login))
            {
                return true;
            }

            return false;
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
