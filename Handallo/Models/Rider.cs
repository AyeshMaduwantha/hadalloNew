using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Models
{
    public class Rider
    {
        public int CustomerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String MobileNo { get; set; }
        public String Pass_word { get; set; }
        public String VerifiCode { get; set; }
        public Boolean Validated { get; set; }
        public String LicenseNo { get; set; }
        public String Nic { get; set; }


    }
}
