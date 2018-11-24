using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DemoApp.Services;
using Handallo.Global;
using Handallo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Handallo.DataProvider
{
    public class ShopOwnerDataProvider : IShopOwnerDataProvider
    {
        private readonly String connectionString;
       // private IConfiguration _config;

        public ShopOwnerDataProvider()
        {
            // connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
            /////connectionString = "Server=tcp: handallo.database.windows.net,1433; Initial Catalog = Handallo;Database=handallo; User ID = Handallo.336699; Password = 16xand99x.Trusted_Connection=True;MultipleActiveResultSets=true";

            //this._config = config;
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

        public ShopUserModel LoginShopOwner(Login login)
        {
            String checkUserName;
            login.Pass_word = HashAndSalt.HashSalt(login.Pass_word);

            var email = login.Email;
            var password = login.Pass_word;

            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT FirstName FROM ShopOwner WHERE Email = @Email AND Pass_word = @Pass_word";
                dbConnection.Open();
                checkUserName = dbConnection.QueryFirstOrDefault<String>(sQuery, new { @Email = email, @Pass_word = password });


            

                if (String.IsNullOrEmpty(checkUserName))
                {
                    return null;
                }
                else
                {
                    string OwnerID;
                    string sQuery1 = "SELECT ShopOwnerId from ShopOwner where Email = @email";
                    OwnerID = dbConnection.QueryFirstOrDefault<String>(sQuery1, new { @Email = email });
                    string sQuery2 = "SELECT * FROM Shop WHERE OwnerId = @OwnerID";
                    try
                    {
                        dynamic shop = dbConnection.QueryFirst(sQuery2, new {OwnerId = OwnerID});




                        ShopUserModel shopuser = null;
                        shopuser = new ShopUserModel
                        {
                            ShopId = shop.ShopId,
                            UId = OwnerID,
                            Name = checkUserName,
                            Email = email,
                            Description = shop.Des_cription,
                            Location = shop.Lo_cation,
                            ShopName = shop.ShopName,
                            Url = shop.url
                        };

                        return shopuser;
//                        String Token = BuildShopUserToken(shopuser);
//                        return new OkObjectResult(new { token = Token });
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                    /* var method = typeof(TokenCreator).GetMethod("createToken");
                     var action = (Action<TokenCreator>)Delegate.CreateDelegate(typeof(Action<TokenCreator>), method);
                     action(user);*/

                } //TokenCreator tokencreator = new TokenCreatorC();

                //return tokencreator.createToken(user);
            }
        }

       


        public UserModel RegisterShopOwner(ShopOwner shopowner)
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
                    String VerifiCode = VerifiCodeGenarator.CreateRandomPassword();
                    shopowner.VerifiCode = VerifiCode;
                    shopowner.Validated = false;
                    string sQuery = "INSERT INTO ShopOwner(FirstName,LastName,Pass_word,Email,MobileNo,VerifiCode,Validated)" +
                                    "VALUES(@FirstName,@LastName,@Pass_word,@Email,@MobileNo,@VerifiCode,@Validated)";

                    dbConnection.Open();
                    dbConnection.Execute(sQuery, shopowner);
                    dbConnection.Close();
                               
                    SendMail(email,VerifiCode);

                    string sQuery1 = "SELECT ShopOwnerId from ShopOwner where Email = @email";
                    string ID = dbConnection.QueryFirstOrDefault<String>(sQuery1, new { @Email = email });
                    
                    UserModel user = null;
                    user = new UserModel { Id = ID, Name = shopowner.FirstName, Email = shopowner.Email };
                    //String Token = BuildToken(user);
                    //return new OkObjectResult(new { token = Token });
                    return user;


                }

                return null;
            }            
            /* var method = typeof(TokenCreator).GetMethod("createToken");
             var action = (Action<TokenCreator>)Delegate.CreateDelegate(typeof(Action<TokenCreator>), method);
             action(user);*/

            //TokenCreator tokencreator = new TokenCreatorC();
            //return tokencreator.createToken(user);

           

        }

        

        async void SendMail(String mail, string VerifiCode)
        {

            Senders emailsender = new Senders();
            await emailsender.SendEmail(mail, VerifiCode);

        }

        

    }

}

