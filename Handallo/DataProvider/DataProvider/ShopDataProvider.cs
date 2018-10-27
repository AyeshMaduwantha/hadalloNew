using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Handallo.DataProvider.IDataProvider;
using Handallo.Global.Images;
using Handallo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Handallo.DataProvider.DataProvider
{
    public class ShopDataProvider : IShopDataProvider
    {
        private readonly String connectionString;
   

        public ShopDataProvider()
        {
             //connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
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

        public dynamic viewShops()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Shop";
                dbConnection.Open();

                return dbConnection.Query(sQuery);
            }


        }



        public async Task<IActionResult> RegisterShop(Shop shop)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var email = shop.Email;
                long number;
                string sQuery0 = "SELECT ShopName FROM Shop WHERE Email = @email";
                dbConnection.Open();
                String result = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @Email = email});
                dbConnection.Close();

                if (string.IsNullOrEmpty(result))
                {
                    string sQuery = "INSERT INTO Shop(ShopName,Des_cription,Email,MobileNo,Lo_cation)" +
                                    "VALUES(@ShopName,@Des_cription,@Email,@MobileNo,@Lo_cation)";

                    dbConnection.Open();
                    dbConnection.Execute(sQuery, shop);
                    dbConnection.Close();
                    string sQuery1 = "SELECT ShopId FROM Shop WHERE Email = @email";
                    dbConnection.Open();
                    String result2 = dbConnection.QueryFirstOrDefault<String>(sQuery1, new { @Email = email });
                    number = Int64.Parse(result2);
                    Image toupload = new Image(shop.image,number);
                    return await UploadImage(toupload);
                    


                }
            }

            return new BadRequestResult();
        }

        public async Task<IActionResult> UploadImage(Image toupload)
        {
            ShopLogoWriter shopLogoWriter = new ShopLogoWriter();
            var result = await shopLogoWriter.UploadImage(toupload);
            return new ObjectResult(result);
        }

        public String  DownloadImage(int imageid)
        {
            using(IDbConnection dbConnection = Connection)
            {
                //imageid = ImageView;
                
                string sQuery0 = "SELECT path FROM Shop WHERE ShopId = @imageid";
                dbConnection.Open();
                //String path = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @ShopId = shopid });
                //String path = dbConnection.Execute(sQuery0);
                String Path = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { imageid = @imageid });

                return Path;
            } 
        }
    }
}

/*
 * Des_cription: "zxczxczx"
Email: "zxczxc@dvd.com"
Lo_cation: "vcxvxcv"
PhoneNo: "zxczxcx"
ShopName: "xzcxz"
 */
