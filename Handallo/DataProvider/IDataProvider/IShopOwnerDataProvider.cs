using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.DataProvider
{
    interface IShopOwnerDataProvider
    {

        Boolean RegisterShopOwner(ShopOwner shopowner);
        Boolean LoginShopOwner(Login login);

    }
}
