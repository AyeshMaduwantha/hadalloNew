using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Handallo.DataProvider.IDataProvider;
using Handallo.Models;
using Newtonsoft.Json;

namespace Handallo.DataProvider.DataProvider
{
    public class ComplainsDataProvider : IComplainsDataProvider
    {

        private readonly String connectionString;
        public ComplainsDataProvider()
        {
            //connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
            ////connectionString = "Server=tcp: handallo.database.windows.net,1433; Initial Catalog = Handallo;Database=handallo; User ID = Handallo.336699; Password = 16xand99x.Trusted_Connection=True;MultipleActiveResultSets=true";

        }

        //public IDbConnection Connection
        //{
        //    get
        //    {
        //        return new SqlConnection(connectionString);
        //    }
        //}

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public dynamic viewAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Complains";
                dbConnection.Open();

                return dbConnection.Query(sQuery);
            }


        }
    }
}
