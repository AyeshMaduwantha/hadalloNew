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

        public ShopLogoWriter()
        {
            connectionString = "Server=DESKTOP-ALMQ9QA\\SQLEXPRESS;Database=handallo;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public SqlConnection Connection
        {
            get { return new SqlConnection(connectionString); }
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            if (CheckIfImageFile(file))
            {
                return await WriteFile(file);
            }

            return "Invalid image file";
        }

        private bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return ImageWriterHelper.GetImageFormat(fileBytes) != ImageWriterHelper.ImageFormat.unknown;
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            String fileName;
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

                Image image = new Image(path);

                toDb(image);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return fileName;
        }

        public void toDb(Image image)
        {
            string path = image.path;

            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO shop(path)" + "VALUES(@path)";
                //SqlCommand cmd = new SqlCommand(sQuery, Connection);
                //cmd.Parameters.Add("fileName", sqlDbType: SqlDbType.NVarChar).Value = fileName;
                //cmd.Parameters.Add("path", sqlDbType: SqlDbType.NVarChar).Value = path;

                // SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

                dbConnection.Open();
                dbConnection.Execute(sQuery, new {path = path});
            }


        }


    }
}
