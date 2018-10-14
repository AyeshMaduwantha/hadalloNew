using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Handallo.Models
{
    public class Shop
    {
        public String ShopName { get; set; }
        public String Des_cription { get; set; }
        public String Email { get; set; }
        public String MobileNo { get; set; }
        public String Lo_cation { get; set; }

        public IFormFile image { get; set; }

    }
}


/*
 *     data: {
      ShopName: "",
      Des_cription: "",
      PhoneNo: "",
      Email: "",
      Lo_cation: ""
    }
 */
