using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handallo.DataProvider;
using Handallo.DataProvider.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {

        public readonly ShopDataProvider _ShopDataProvider;

        public ShopController()
        {
            _ShopDataProvider = new ShopDataProvider();
        }
        // GET: api/Shop
        [HttpGet]
        public dynamic Get()
        {
            return _ShopDataProvider.viewShops();
        }

        // GET: api/Shop/5
        /*  [HttpGet("{id}", Name = "Get")]
          public string Get(int id)
          {
              return "value";
          }*/

        [HttpPost("register")]
        public async Task<IActionResult> RegisterShop([FromForm]Shop shop)
        {
           return await  _ShopDataProvider.RegisterShop(shop);
            
        }

       /* [HttpPost("logo")]
        public async Task<IActionResult> UploadImage(Shop shop)
        {
            return await _ShopDataProvider.RegisterShop(shop);
        }
        */


    }
}
