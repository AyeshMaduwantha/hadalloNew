using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.DataProvider
{
    interface IAdministerDataProvider
    {
        Administer GetAdminister(int RiderId);
        Boolean RegisterAdmin(Administer administer);
        Boolean LoginAdmin(Login login);



    }
}
