using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Models
{
    public class Deliver
    {
        public String DeliverId { get; set; }
        public SingleOrder order { get; set; }
        public int Rating { get; set; }
        public String PaymentMethod { get; set; }
        public String OrderStatus { get; set; }
        [DataType(DataType.Date)]
        public DateTime DeliverTime { get; set; }
        public Customer Customer { get; set; }
        
    }
}
