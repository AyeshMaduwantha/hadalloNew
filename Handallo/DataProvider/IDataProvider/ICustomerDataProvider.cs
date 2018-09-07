using Handallo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handallo.DataProvider
{
    interface ICustomerDataProvider
    {
        Boolean RegisterCustomer(Customer customer);
        Boolean LoginCustomer(Login login);


    }
}
