using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Handallo.Models;

namespace Handallo.DataProvider
{
    public class ShopOwnerDataProvider : IShopOwnerDataProvider
    {
        private readonly String connectionString;

        public ShopOwnerDataProvider()
        {
            connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public bool LoginShopOwner(Login login)
        {
            throw new NotImplementedException();
        }

        public bool RegisterShopOwner(ShopOwner shopowner)
        {
            throw new NotImplementedException();
        }
    }

}

