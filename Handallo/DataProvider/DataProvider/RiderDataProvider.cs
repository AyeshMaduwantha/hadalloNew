using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DemoApp.Services;
using Handallo.Global;
using Handallo.Global.Images;
using Handallo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider
{
    public class RiderDataProvider : IRiderDataProvider
    {
        private readonly String connectionString;

       public RiderDataProvider()
        {
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
            ////connectionString = "Server=tcp: handallo.database.windows.net,1433; Initial Catalog = Handallo;Database=handallo; User ID = Handallo.336699; Password = 16xand99x.Trusted_Connection=True;MultipleActiveResultSets=true";
            //connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
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

        public UserModel LoginRider(Login login)
        {
            String checkUserName;
            login.Pass_word = HashAndSalt.HashSalt(login.Pass_word);

            var email = login.Email;
            var password = login.Pass_word;
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT FirstName FROM Rider WHERE Email = @Email AND Pass_word = @Pass_word";
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

        public async Task<IActionResult>RegisterRider(Rider rider)
        {
            long number;
            var email = rider.Email;
            rider.Pass_word = HashAndSalt.HashSalt(rider.Pass_word);
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery0 = "SELECT FirstName FROM Rider WHERE Email = @email";
                dbConnection.Open();
                String result = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @Email = email });
                dbConnection.Close();
                if (string.IsNullOrEmpty(result))
                {
                    rider.VerifiCode = VerifiCodeGenarator.CreateRandomPassword();
                    rider.Validated = false;
                    string sQuery = "INSERT INTO Rider(FirstName,LastName,Pass_word,Email,MobileNo,VerifiCode,Validated,LicenseNo,Nic)" +
                                    "VALUES(@FirstName,@LastName,@Pass_word,@Email,@MobileNo,@VerifiCode,@Validated,@LicenseNo,@Nic)";

                    dbConnection.Open();
                    dbConnection.Execute(sQuery, rider);
                    dbConnection.Close();
                    string sQuery1 = "SELECT RiderId FROM Rider WHERE Email = @email";
                    dbConnection.Open();
                    String result2 = dbConnection.QueryFirstOrDefault<String>(sQuery1, new { @Email = email });
                    number = Int64.Parse(result2);
                    Image toupload = new Image(rider.image, number);
                    Senders emailsender = new Senders();
                    await emailsender.SendEmail(email, rider.VerifiCode);
                    return await UploadImage(toupload);

                }

                return new ConflictResult();
            }



        }

        public dynamic ViewRiders()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Rider";
                dbConnection.Open();

                return dbConnection.Query(sQuery);
            }


        }













        public async Task<IActionResult> UploadImage(Image toupload)
        {
            RiderImageWriter riderImagewriiter = new RiderImageWriter();
            var result = await riderImagewriiter.UploadImage(toupload);
            return new ObjectResult(result);
        }

        public String DownloadImage(int imageid)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //imageid = ImageView;

                string sQuery0 = "SELECT path FROM Rider WHERE RiderId = @imageid";
                dbConnection.Open();
                //String path = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @ShopId = shopid });
                //String path = dbConnection.Execute(sQuery0);
                String Path = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { imageid = @imageid });

                return Path;
            }
        }

    }
}
