using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Handallo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Handallo.Global.Images
{
    public class ShopLogoWriter : IShopLogoWriter
    {
        private readonly string connectionString;
        private IShopLogoWriter _shopLogoWriterImplementation;

        public ShopLogoWriter()
        {
            //connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
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
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Shops", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }

                Image imageupload = new Image(path,Id);

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
            long ShopId = unchecked((int)imageUpload.ShopId);

            using (IDbConnection dbConnection = Connection)
            {
                string url = "https://localhost:44371/api/Shop/download/" + ShopId;
                string sQuery = "UPDATE shop SET path = @path WHERE ShopId = @ShopId ;"; //update product set CategoriesId = 2 where Categories = 'ab'
                string sQuery1 = "UPDATE shop SET url = @url WHERE ShopId = @ShopId ;";

                //SqlCommand cmd = new SqlCommand(sQuery, Connection);
                //cmd.Parameters.Add("fileName", sqlDbType: SqlDbType.NVarChar).Value = fileName;
                //cmd.Parameters.Add("path", sqlDbType: SqlDbType.NVarChar).Value = path;

                // SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

                dbConnection.Open();
                dbConnection.Execute(sQuery, new {path = path,ShopId = ShopId});
                dbConnection.Execute(sQuery1, new { url = url, ShopId = ShopId });
            }


        }



    }
}
