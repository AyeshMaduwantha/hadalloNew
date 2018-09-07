using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handallo.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly CustomerDataProvider _CustomerDataProvider;

        public CustomerController()
        {
            _CustomerDataProvider = new CustomerDataProvider();
        }

        // GET api/values
        /*[HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            return _CustomerDataProvider.Getustomer(id);
        }

        // POST api/values
        [HttpPost("register")]
        public ActionResult Post([FromBody] Customer customer)
        {
            if (_CustomerDataProvider.RegisterCustomer(customer))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public ActionResult Post([FromBody] Login login)
        {
            if (_CustomerDataProvider.LoginCustomer(login))
            {
                return Ok();
            }

            return BadRequest();
        }

        /*   // PUT api/values/5
           [HttpPut("{id}")]
           public void Put(int id, [FromBody] string value)
           {
           }

           // DELETE api/values/5
           [HttpDelete("{id}")]
           public void Delete(int id)
           {
           }*/
    }
}
