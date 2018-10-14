using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Handallo.Models
{
    public class Image
    {
        public String path { get; set; }
        public long ShopId { get; set; }

        public IFormFile image { get; set; }



        public Image(String path,long ShopId)
        {

            this.path = path;
            this.ShopId = ShopId;
        }

        public Image(IFormFile image,long ShopId )
        {
            this.image = image;
            this.ShopId = ShopId;
        }
    }
}
