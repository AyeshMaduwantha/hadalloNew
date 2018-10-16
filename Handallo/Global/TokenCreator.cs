using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handallo.Models;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Handallo.Global
{
    public class TokenCreator

    {

        private IConfiguration _config;

        private TokenCreator(IConfiguration config)
        {
            _config = config;
        }



        public IActionResult createToken(UserModel user)
        {
            IActionResult response;

                var tokenString = BuildToken(user);
                response = new OkObjectResult(new{token = tokenString});

            return response;
        }

        private string BuildToken(UserModel user)
        {
      
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
