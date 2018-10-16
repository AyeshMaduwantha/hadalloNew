using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Handallo.Global;
using Handallo.Models;

namespace Handallo.DataProvider
{
    public class ShopOwnerDataProvider : IShopOwnerDataProvider
    {
        private readonly String connectionString;
        private String checkExist;

        public ShopOwnerDataProvider()
        {
            // connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
            //connectionString = "Server=tcp: handallo.database.windows.net,1433; Initial Catalog = Handallo;Database=handallo; User ID = Handallo.336699; Password = 16xand99x.Trusted_Connection=True;MultipleActiveResultSets=true";
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

        public bool LoginShopOwner(Login login)
        {
            login.Pass_word = HashAndSalt.HashSalt(login.Pass_word);

            var o = login.Email;
            var i = login.Pass_word;

            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT FirstName FROM ShopOwner WHERE Email = @Email AND Pass_word = @Pass_word";
                dbConnection.Open();
                checkExist = dbConnection.QueryFirstOrDefault<String>(sQuery, new { @Email = o, @Pass_word = i });


            }

            if (String.IsNullOrEmpty(this.checkExist))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool RegisterShopOwner(ShopOwner shopowner)
        {
            var email = shopowner.Email;
            shopowner.Pass_word = HashAndSalt.HashSalt(shopowner.Pass_word);
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery0 = "SELECT FirstName FROM ShopOwner WHERE Email = @email";
                dbConnection.Open();
                String result = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @Email = email });
                dbConnection.Close();

                if (string.IsNullOrEmpty(result))
                {
                    string sQuery = "INSERT INTO ShopOwner(FirstName,LastName,Pass_word,Email,MobileNo)" +
                                    "VALUES(@FirstName,@LastName,@Pass_word,@Email,@MobileNo)";

                    dbConnection.Open();
                    dbConnection.Execute(sQuery, shopowner);
                    return true;

                }
            }
            return false;
        }
    }

}

