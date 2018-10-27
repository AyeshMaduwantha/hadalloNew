using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DemoApp.Services;
using Handallo.Global;
using Handallo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Handallo.DataProvider
{
    public class CustomerDataProvider : ICustomerDataProvider
    {
        private readonly String connectionString;

        public CustomerDataProvider()
        {
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
            //connectionString = "Server=tcp: handallo.database.windows.net,1433; Initial Catalog = Handallo;Database=handallo; User ID = Handallo.336699; Password = 16xand99x.Trusted_Connection=True;MultipleActiveResultSets=true";
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

        public Customer GetCustomer(int CustomerId)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Customer"
                                + " WHERE CustomerId = @Id";
                dbConnection.Open();

                return dbConnection.Query<Customer>(sQuery, new { Id = CustomerId }).FirstOrDefault();
            }
        }

        public async Task<Boolean> RegisterCustomer(Customer customer)
        {
            
            var email = customer.Email;
            customer.Pass_word = HashAndSalt.HashSalt(customer.Pass_word);
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery0 = "SELECT FirstName FROM Customer WHERE Email = @email";
                dbConnection.Open();
                String result = dbConnection.QueryFirstOrDefault<String>(sQuery0, new { @Email = email });
                dbConnection.Close();

                if (string.IsNullOrEmpty(result))
                {
                    customer.VerifiCode = VerifiCodeGenarator.CreateRandomPassword();
                    customer.Validated  = false;
                    string sQuery = "INSERT INTO Customer(FirstName,LastName,Pass_word,Email,MobileNo,VerifiCode,Validated)" +
                                    "VALUES(@FirstName,@LastName,@Pass_word,@Email,@MobileNo,@VerifiCode,@Validated)";

                    dbConnection.Open();
                    //dbConnection.Execute(sQuery, new { customer.FirstName = FirstName , VerifiCode = vCode });
                    dbConnection.Execute(sQuery, customer);
                    Senders emailsender = new Senders();
                   await emailsender.SendEmailAsync("csanjeewag@gmail.com", customer.VerifiCode);
                    return true;

                }
            }
            return false;
        }

        
        public UserModel LoginCustomer(Login login)
        {
            String checkUserName;
            login.Pass_word = HashAndSalt.HashSalt(login.Pass_word);

            var email = login.Email;
            var password = login.Pass_word;

            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT FirstName FROM Customer WHERE Email = @Email AND Pass_word = @Pass_word";
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


    }
}
