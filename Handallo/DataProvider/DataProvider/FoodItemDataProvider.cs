using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Handallo.Global.Images;
using Handallo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider.DataProvider
{
    public class FoodItemDataProvider
    {
        private readonly String connectionString;
        private String checkExist;

        public FoodItemDataProvider()
        {
            //connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
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

        public async Task<IActionResult> AddFoodItems(FoodItem fooditem)
        {
            using (IDbConnection dbConnection = Connection)
            {
                int fooditemid = fooditem.FoodItemId;

                string sQuery = 
                                "INSERT INTO FoodItem(FoodName,Description,IsDinner,IsBreakfast,IsLunch,Availability)" +
                                " VALUES(@FoodName,@Description,@IsDinner,@IsBreakfast,@IsLunch,@Availability);" +
                                "SELECT CAST(SCOPE_IDENTITY() as int) ";

                string sQuery1 = "INSERT INTO ShopHasFoodItem(ShopId,FoodItemId) VALUES(@ShopId,@FoodItemId)";

                string sQuery2 = "INSERT INTO FoodItemHasPrices(FoodItemId,SmallUnitPrice,MediumUnitPrice,LargeUnitPrice) " +
                                 "VALUES(@FoodItemId,@SmallUnitPrice,@MediumUnitPrice,@LargeUnitPrice)";
                string sQuery3 = "INSERT INTO FoodItemHasType(FoodItemId,IsVegi,IsNonVegi,IsRice,IsBeverage)" +
                                 "VALUES(@FoodItemId,@IsVegi,@IsNonVegi,@IsRice,@IsBeverage)";

                //dbConnection.Open();
                //dbConnection.Execute(sQuery, fooditem);
                //dbConnection.Close();
                //string sQuery1 =
                //    "SELECT MAX(FoodItemId) FROM FoodItem";
                dbConnection.Open();
                string result2 = dbConnection.QueryFirstOrDefault<string>(sQuery,
                    new
                    {
                        FoodName = fooditem.FoodName,
                        Description = fooditem.Description,
                        IsDinner= fooditem.IsDinner,
                        IsBreakfast = fooditem.IsBreakFast,
                        IsLunch = fooditem.IsLunch,
                        Availability = fooditem.Availability,
                    });
                dbConnection.Close();
                dbConnection.Open();
                long number = Int64.Parse(result2);
                dbConnection.Execute(sQuery1, new {  FoodItemId = number, ShopId = fooditem.ShopId });
                dbConnection.Close();
                dbConnection.Open();
                dbConnection.Execute(sQuery2,
                      new
                      {
                    FoodItemId = number,
                    SmallUnitPrice = fooditem.SmallUnitPrice,
                    MediumUnitPrice = fooditem.MediumUnitPrice,
                    LargeUnitPrice = fooditem.LargeUnitPrice
                      });
                dbConnection.Close();
                dbConnection.Open();
                dbConnection.Execute(sQuery3, 
                    new
                    {
                        FoodItemId = number,
                        IsVegi = fooditem.IsVegi,
                        IsNonVegi = fooditem.IsNonVegi,
                        IsRice = fooditem.IsRice,
                        IsBeverage = fooditem.IsBeverage
                    });
                //int result2 = dbConnection.ExecuteScalar<int>(sQuery);

                Image toupload = new Image(fooditem.Image, number);
                return await UploadImage(toupload);


            }

          
        }

        public async Task<IActionResult> UploadImage(Image toupload)
        {
            FoodItemWriter FoodItemWriter = new FoodItemWriter();
            var result = await FoodItemWriter.UploadImage(toupload);
            return new ObjectResult(result);
        }


        public string DownloadFoodItemImage(int imageid)
        {
            using (IDbConnection dbConnection = Connection)
            {
                //imageid = ImageView;

                string sQuery0 = "SELECT path FROM FoodItem WHERE FoodItemId = @imageid";
                dbConnection.Open();
                //String path = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @ShopId = shopid });
                //String path = dbConnection.Execute(sQuery0);
                String Path = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { imageid = @imageid });

                return Path;
            }

        }
    }
}
