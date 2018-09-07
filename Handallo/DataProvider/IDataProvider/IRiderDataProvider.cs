using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.DataProvider
{
    interface IRiderDataProvider
    {
        Boolean RegisterRider(Rider rider);
        Boolean LoginRider(Login login);
    }
}
