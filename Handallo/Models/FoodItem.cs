using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Models
{
    public class FoodItem
    {
        public int ShopId { get; set; }
        public int TypeId { get; set; }
        public int ItemId { get; set; }
        public Boolean IsDinner { get; set; }
        public Boolean IsLunch { get; set; }
        public Boolean IsBreakFast { get; set; }

        public String  Path { get; set; }
        public String  Url { get; set; }

        public Boolean Avaliability { get; set; }

    }
}
