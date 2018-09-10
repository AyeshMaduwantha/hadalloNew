using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handallo.Models;
using Newtonsoft.Json;

namespace Handallo.DataProvider.IDataProvider
{
    interface IComplainsDataProvider
    {
       dynamic viewAll();
    }
}
