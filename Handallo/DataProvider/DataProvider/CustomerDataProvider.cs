using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Handallo.Models;

namespace Handallo.DataProvider
{
    public class CustomerDataProvider : ICustomerDataProvider
    {
        private readonly String connectionString;

        public CustomerDataProvider()
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

        public Customer Getustomer(int CustomerId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Customer"
                                + " WHERE CustomerId = @Id";
                dbConnection.Open();

                return dbConnection.Query<Customer>(sQuery, new { Id = CustomerId }).FirstOrDefault();
            }
        }

        public bool RegisterCustomer(Customer customer)
        {

            throw new NotImplementedException();
        }

        public bool LoginCustomer(Login login)
        {
            throw new NotImplementedException();
        }


    }
}
