using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Models
{
    public class ShopUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string UId { get; set; }
        public int ShopId { get; set; }

        public string ShopName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Url { get; set; }

        public String Lat { get; set; }
        public String Lng { get; set; }
    }
}
