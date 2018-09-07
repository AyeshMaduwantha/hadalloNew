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
    public class RiderController : ControllerBase
    {
        public readonly RiderDataProvider _RiderDataProvider;

        public RiderController()
        {
            _RiderDataProvider = new RiderDataProvider();
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
        public ActionResult Post([FromBody] Rider rider)
        {
            if (_RiderDataProvider.RegisterRider(rider))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public ActionResult Post([FromBody] Login login)
        {
            if (_RiderDataProvider.LoginRider(login))
            {
                return Ok();
            }

            return BadRequest();
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
    }
}
