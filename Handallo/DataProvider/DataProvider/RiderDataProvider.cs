using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Handallo.Models;

namespace Handallo.DataProvider
{
    public class RiderDataProvider : IRiderDataProvider
    {
        private readonly String connectionString;

       public RiderDataProvider()
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

        public bool LoginRider(Login login)
        {
            throw new NotImplementedException();
        }

        public bool RegisterRider(Rider rider)
        {
            throw new NotImplementedException();
        }
    }
}
