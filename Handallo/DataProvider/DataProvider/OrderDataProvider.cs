using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Dapper;
using Handallo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Handallo.DataProvider.DataProvider
{
    public class OrderDataProvider
    {
        public string connectionString;
        int FoodItemId = 0;
        Boolean IsSmallPotion = false;
        Boolean IsMediumPotion = false;
        Boolean IsLargePotion = false;

        public OrderDataProvider()
        {
            //connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
            /////connectionString = "Server=tcp: handallo.database.windows.net,1433; Initial Catalog = Handallo;Database=handallo; User ID = Handallo.336699; Password = 16xand99x.Trusted_Connection=True;MultipleActiveResultSets=true";
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public async Task<IActionResult> PlaceOrder(SingleOrder order)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO SingleOrder(OrderDate,OrderTime,PaymentMethod)" +
                                "VALUES(@OrderDate,@OrderTime,@PaymentMethod)" +
                                "SELECT CAST(SCOPE_IDENTITY() as int)";

                dbConnection.Open();
                string result2 = dbConnection.QueryFirstOrDefault<string>(sQuery, new { OrderDate = order.OrderDate.Date, OrderTime = order.OrderTime.TimeOfDay,PaymentMethod = order.PaymentMethod });

                dbConnection.Close();
                dbConnection.Open();
                long orderid = Int64.Parse(result2);

                try
                {
                    foreach (var item in order.Order)
                    {

                      FoodItemId = item.FoodItemId;
                      IsSmallPotion = item.IsSmallPotion;
                      IsMediumPotion = item.IsMediumPotion;
                      IsLargePotion = item.IsLargePotion;
                    }

                }
                catch (Exception e)
                {

                }

                string processQuery = "INSERT INTO  SingleOrderHasFoodItem(OrderId,FoodItemId,IsSmallPotion,IsMediumPotion,IsLargePotion)" +
                                      "VALUES(@OrderId,@FoodItemId,@IsSmallPotion,@IsMediumPotion,@IsLargePotion)";
                dbConnection.Execute(processQuery, new
                {
                    OrderID = orderid,
                    FoodItemId = this.FoodItemId,
                    IsSmallPotion = this.IsSmallPotion,
                    IsMediumPotion = this.IsMediumPotion,
                    IsLargePotion =  this.IsLargePotion,
                });

                dbConnection.Close();

                if (Deliver(order, orderid))
              {
                return new OkResult();
              }

              return new ConflictResult();
            }
            }
            


        //DeliverId int identity(1,1) not null primary key,
        //    OrderId int foreign key references SingleOrder(OrderId),
        //CustomerId int foreign key references Customer(CustomerId),
        //RiderId int foreign key references Rider(RiderId),
        //IsAdminConfirmed BIT,
        //    IsRiderConfirmed BIT,
        //PaymentStatus nvarchar(20), 

        private Boolean Deliver(SingleOrder order,long orderid)
        {
            using (IDbConnection dbConnection = Connection)
            {
                String sQuery = "INSERT INTO Deliver(OrderId,CustomerId,IsAdminConfirmed,PaymentStatus)" +
                                "VAlUES(@OrderId,@CustomerId,@IsAdminConfirmed,@PaymentStatus)";
                String sQuery1 = "INSERT INTO CustomerHasLocation(CustomerId,CurrentLng,CurrentLat) " +
                                 "VALUES(@CustomerId,@CurrentLng,@CurrentLat)";

                dbConnection.Open();

                dbConnection.Execute(sQuery, new {
                    OrderID = orderid,
                    CustomerId = order.Customer.CustomerId,
                    IsAdminConfirmed = false,
                    PaymentStatus = "Not paid",

                });

                dbConnection.Close();

                dbConnection.Open();
                dbConnection.Execute(sQuery1, new
                {
                    CustomerId = order.Customer.CustomerId,
                    CurrentLng = order.Customer.CurrentLng,
                    CurrentLat = order.Customer.CurrentLat,

                });

                dbConnection.Close();
            }

     

            return true;

        }

        public IActionResult ViewOrders()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Deliver";
                dbConnection.Open();

                return new JsonResult(dbConnection.Query(sQuery));
            }
        }

        public IActionResult ApproveOrder(SingleOrder order)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE Deliver SET IsAdminConfirmed = true WHERE OrderId = OrderId";
                dbConnection.Execute(sQuery, new { IsAdminConfirmed = true, OrderId = order.OrderId });
            }


            return new OkResult();
        }

        public dynamic ViewApprovedOrders()
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Deliver WHERE";
                dbConnection.Open();

                return dbConnection.Query(sQuery);
            }

        }
    }
}
