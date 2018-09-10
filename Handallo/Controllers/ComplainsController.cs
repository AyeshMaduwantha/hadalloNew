using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handallo.DataProvider.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainsController : ControllerBase
    {
        // GET: api/Complains
        public readonly ComplainsDataProvider _ComplainsDataProvider;

        public ComplainsController()
        {
            _ComplainsDataProvider = new ComplainsDataProvider();
        }




        [HttpGet]
        public dynamic Get()
        {
           return _ComplainsDataProvider.viewAll();
        }

        // GET: api/Complains/5
       /* [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Complains
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Complains/5
        [HttpPut("{id}")]
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
