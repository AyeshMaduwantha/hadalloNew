using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.DataProvider
{
    interface IShopOwnerDataProvider
    {

        UserModel RegisterShopOwner(ShopOwner shopowner);
        UserModel LoginShopOwner(Login login);

    }
}
