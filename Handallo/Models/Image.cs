using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.Models
{
    public class Image
    {
        public String path { get; set; }

        public Image(String path)
        {

            this.path = path;
        }
    }
}
