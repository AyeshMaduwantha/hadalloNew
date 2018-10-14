using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Handallo.Global.Images
{
    interface IShopLogoWriter
    {
       Task<string> UploadImage(IFormFile file);
    }
}
