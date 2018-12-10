using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Models
{
    public class SingleOrder
    {
        public int OrderId { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime OrderTime { get; set; }
        public FoodItem[] Order { get; set; }
        public String PaymentMethod { get; set; }
        public Customer Customer  { get; set; }
    }
}
