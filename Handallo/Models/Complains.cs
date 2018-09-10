using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Models
{
    public class Complains
    {
        public int ComplainsId { get; set; }
        public int OrderId { get; set; }
        public int PersonId { get; set; }
        public String Com_description { get; set; }
        public DateTime Com_date { get; set; }
    }
}
