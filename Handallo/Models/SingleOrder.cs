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

        public Boolean IsSmallPotion { get; set; }
        public Boolean IsMediumPoition { get; set; }
        public Boolean IsLargePotion { get; set; }

    }
}
