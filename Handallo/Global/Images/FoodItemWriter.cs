﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Handallo.Models;
using Microsoft.AspNetCore.Http;

namespace Handallo.Global.Images
{
    public class FoodItemWriter
    {
        private readonly string connectionString;
        private IShopLogoWriter _shopLogoWriterImplementation;

        public FoodItemWriter()
        {
           // connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
            connectionString = "Server=tcp:handallo.database.windows.net;Database=handallo;User ID=Handallo.336699;Password=16xand99x.;Trusted_Connection=false;MultipleActiveResultSets=true";
        }

        public SqlConnection Connection
        {
            get { return new SqlConnection(connectionString); }
        }

        public async Task<string> UploadImage(Image image)
        {
            if (CheckIfImageFile(image))
            {
                return await WriteFile(image);
            }

            return "Invalid image file";
        }

        private bool CheckIfImageFile(Image image)
        {
            IFormFile file = image.image;
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return ImageWriterHelper.GetImageFormat(fileBytes) != ImageWriterHelper.ImageFormat.unknown;
        }

        private async Task<string> WriteFile(Image image)
        {
            String fileName;
            IFormFile file = image.image;
            long Id = image.ShopId;

            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() +
                           extension; //Create a new Name for the file due to security reasons.
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\FoodItem", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }

                Image imageupload = new Image(path, Id);

                toDb(imageupload);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return fileName;
        }

        public void toDb(Image imageUpload)
        {
            string path = imageUpload.path;
            long FoodItemId = unchecked((int)imageUpload.ShopId);
            //int FoodItemId = imageUpload.FoodItemId;
            //https://handallo.azurewebsites.net/api/Shop/downloadfooditem/
            using (IDbConnection dbConnection = Connection)
            {
                string url = "https://handallo.azurewebsites.net/api/Shop/downloadfooditem/" + FoodItemId;
               // string url = "https://localhost:44371/api/Shop/downloadfooditem/" + FoodItemId;
                string sQuery = "UPDATE FoodItem SET path = @path WHERE FoodItemId = @FoodItemId ;"; //update product set CategoriesId = 2 where Categories = 'ab'
                string sQuery1 = "UPDATE FoodItem SET url = @url WHERE FoodItemId = @FoodItemId ;";

                //SqlCommand cmd = new SqlCommand(sQuery, Connection);
                //cmd.Parameters.Add("fileName", sqlDbType: SqlDbType.NVarChar).Value = fileName;
                //cmd.Parameters.Add("path", sqlDbType: SqlDbType.NVarChar).Value = path;

                // SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

                dbConnection.Open();
                dbConnection.Execute(sQuery, new { path = path, FoodItemId = FoodItemId });
                dbConnection.Execute(sQuery1, new { url = url, FoodItemId = FoodItemId });
            }


        }
    }
}
