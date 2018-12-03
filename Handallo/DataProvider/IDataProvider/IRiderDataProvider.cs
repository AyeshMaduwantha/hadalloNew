using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider
{
    interface IRiderDataProvider
    {
        Task<IActionResult> RegisterRider(Rider rider);
        UserModel LoginRider(Login login);
    }
}
