﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Handallo.DataProvider;
using Handallo.DataProvider.DataProvider;
using Handallo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Handallo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly CustomerDataProvider _CustomerDataProvider;
        public readonly OrderDataProvider _OrderDataProvider;
        public readonly FoodItemDataProvider _FooditemdataDataProvider;
        private IConfiguration _config;
        UserModel result;
        public CustomerController(IConfiguration config)
        {
            _CustomerDataProvider = new CustomerDataProvider();
            _OrderDataProvider = new OrderDataProvider();
            _FooditemdataDataProvider = new FoodItemDataProvider();
            _config = config;
        }

        // GET api/values
        /*[HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            return _CustomerDataProvider.GetCustomer(id);
        }

        // POST api/values
        [HttpPost("register")]
        public Task<Boolean> Post([FromBody] Customer customer)
        {
            return _CustomerDataProvider.RegisterCustomer(customer);
        }

        [HttpPost("login")]
        public IActionResult Post([FromBody] Login login)
        {
            
             result = _CustomerDataProvider.LoginCustomer(login);
            if (result == null)
            {
                return new BadRequestResult();
            }

            String token = (BuildToken(result));
            return new OkObjectResult(new {token = token});


        }

        [HttpGet("getfooditem/{fooditemid:int}")]

        public IActionResult GetFoodItem(int fooditemid)
        {
            return _FooditemdataDataProvider.FoodItemDetail(fooditemid);
        }

        [HttpPost("placeorder")]
        public async Task<IActionResult> PlaceOrder([FromBody] SingleOrder singleorder)
        {

            return await _OrderDataProvider.PlaceOrder(singleorder);
            
        }

        private string BuildToken(UserModel user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Acr, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
           
            //return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*   // PUT api/values/5
           [HttpPut("{id}")]
           public void Put(int id, [FromBody] string value)
           {
           }


           // DELETE api/values/5
           [HttpDelete("{id}")]
           public void Delete(int id)
           {
           }*/
    }
}
