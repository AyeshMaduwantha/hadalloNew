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
    public class ShopOwnerController : ControllerBase
    {
        public readonly ShopOwnerDataProvider _ShopOwnerDataProvider;

        public ShopOwnerController()
        {
            _ShopOwnerDataProvider = new ShopOwnerDataProvider();
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
        public Boolean Post([FromBody] ShopOwner shopowner)
        {
            if (_ShopOwnerDataProvider.RegisterShopOwner(shopowner))
            {
                return true;
            }

            return false;
        }

        [HttpPost("login")]
        public Boolean Post([FromBody] Login login)
        {
            if (_ShopOwnerDataProvider.LoginShopOwner(login))
            {
                return true;
            }

            return false;
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
    }
}
