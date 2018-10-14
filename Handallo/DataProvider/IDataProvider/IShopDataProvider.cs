using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handallo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider.IDataProvider
{
    interface IShopDataProvider
    {
        Task<IActionResult> RegisterShop(Shop shop);

        Task<IActionResult> UploadImage(Image image);
    }
}
