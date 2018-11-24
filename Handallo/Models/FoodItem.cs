using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Handallo.Models
{
    public class FoodItem
    {

        public int ShopId { get; set; }
        public int FoodItemId { get; set; }
        public int SmallUnitPrice { get; set; }
        public int MediumUnitPrice { get; set; }
        public int LargeUnitPrice { get; set; }
        public String FoodName { get; set; }
        public String Description { get; set; }
        public Boolean IsDinner { get; set; }
        public Boolean IsLunch { get; set; }
        public Boolean IsBreakFast { get; set; }
        public String  Path { get; set; }
        public String  Url { get; set; }

        public Boolean Availability { get; set; }
        public IFormFile Image { get; set; }

    }
}
