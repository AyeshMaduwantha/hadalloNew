using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider
{
    interface IShopOwnerDataProvider
    {

        UserModel RegisterShopOwner(ShopOwner shopowner);
        ShopUserModel LoginShopOwner(Login login);

    }
}
