using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider
{
    interface ICustomerDataProvider
    {
       Task<Boolean> RegisterCustomer(Customer customer);
        UserModel LoginCustomer(Login login);


    }
}
