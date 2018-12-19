using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DemoApp.Services;
using Handallo.Global;
using Handallo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider
{
    public class AdministerDataProvider : IAdministerDataProvider
    {
        private readonly String connectionString;
        private String checkExist;

        public AdministerDataProvider()
        {
            // connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
           //// connectionString = "Server=tcp: handallo.database.windows.net,1433; Initial Catalog = Handallo;Database=handallo; User ID = Handallo.336699; Password = 16xand99x.Trusted_Connection=True;MultipleActiveResultSets=true";
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


        public Administer GetAdminister(int AdminsterId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Administer"
                                + " WHERE AdministerId = @Id";
                dbConnection.Open();

                return dbConnection.Query<Administer>(sQuery, new { Id = AdminsterId}).FirstOrDefault();
            }
        }

        public bool RegisterAdmin(Administer administer)
        {
            var email = administer.Email;
            administer.Pass_word = HashAndSalt.HashSalt(administer.Pass_word);
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery0 = "SELECT FirstName FROM Administer WHERE Email = @email";
                dbConnection.Open();
                String result = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @Email = email });
                dbConnection.Close();

                if (string.IsNullOrEmpty(result))
                {
                    string sQuery = "INSERT INTO Administer(FirstName,LastName,Pass_word,Email,MobileNo)" +
                                    "VALUES(@FirstName,@LastName,@Pass_word,@Email,@MobileNo)";

                    dbConnection.Open();
                    dbConnection.Execute(sQuery, administer);
                    return true;

                }
            }
            return false;
        }
        public UserModel LoginAdmin(Login login)
        {
            String checkUserName;
            login.Pass_word = HashAndSalt.HashSalt(login.Pass_word);

            var email = login.Email;
            var password = login.Pass_word;

            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT FirstName FROM Administer WHERE Email = @Email AND Pass_word = @Pass_word";
                dbConnection.Open();
                checkUserName = dbConnection.QueryFirstOrDefault<String>(sQuery, new { @Email = email, @Pass_word = password });


            }

            if (String.IsNullOrEmpty(checkUserName))
            {
                return null;
            }
            else
            {
                UserModel user = null;
                user = new UserModel { Name = checkUserName, Email = email };
                return user;
                /* var method = typeof(TokenCreator).GetMethod("createToken");
                 var action = (Action<TokenCreator>)Delegate.CreateDelegate(typeof(Action<TokenCreator>), method);
                 action(user);*/

                //TokenCreator tokencreator = new TokenCreatorC();
                //return tokencreator.createToken(user);
            }

        }

        public IActionResult ViewComplains(int shopid)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {

                    String sQuery = "SELECT * FROM ShapHasComplains WHERE ShopID = @shopid";

                    dbConnection.Open();

                    return new OkObjectResult(dbConnection.QueryFirstOrDefault<String>(sQuery, new {ShopId = shopid}));

                }
            }
            catch (Exception e)
            {
                return new ConflictResult();
            }


            
        }


        public IActionResult ViewRatings(int shopid)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {

                    String sQuery = "SELECT * FROM ShapHasRatings WHERE ShopID = @shopid";

                    dbConnection.Open();

                    return new OkObjectResult(dbConnection.QueryFirstOrDefault<String>(sQuery, new { ShopId = shopid }));

                }
            }
            catch (Exception e)
            {
                return new ConflictResult();
            }



        }


        public IActionResult SendEmailsToShop(Email email)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    String sQuery =
                        "INSERT INTO ShopHasMails(ShopId,Email)Values(@ShopId,@Email) WHERE ShopId = @ShopId";

                    dbConnection.QueryFirstOrDefault(sQuery, new {ShopId = email.Id});

                    return new OkResult();
                }
                 
            }
            catch (Exception e)
            {
                return new ConflictResult();
            }



        }


        public async Task<IActionResult> SendEmail(Email email)
        {
           Senders sender = new Senders();

           return await (sender.SendEmail(email.Message,"xdcfvg"));


        }
    }
}
