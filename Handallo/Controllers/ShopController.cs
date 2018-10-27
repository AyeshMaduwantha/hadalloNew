using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Handallo.DataProvider;
using Handallo.DataProvider.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet, Authorize]
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

        [HttpGet]
        [Route("download/{id:int}")]
        public Task<IActionResult> Getfiles(int id)
        {

            String path = _ShopDataProvider.DownloadImage(id);
            return Download(path);

        }

       /* [HttpPost("logo")]
        public async Task<IActionResult> UploadImage(Shop shop)
        {
            return await _ShopDataProvider.RegisterShop(shop);
        }
        */


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
                {".csv", "text/csv"}
            };
        }


    }
}
