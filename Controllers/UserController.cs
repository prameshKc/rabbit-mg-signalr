using angularDotnet.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace angularDotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {


        private readonly IUser user;
        private readonly IConfiguration configuration;



        public UserController(IUser user, IConfiguration configuration)
        {
            this.user = user;
            this.configuration = configuration;

        }

        [HttpGet("ping")]
        public IActionResult Ping(){
           Consume();
            return Ok("Pong");
        }

        [HttpGet("Publish")]
        public IActionResult Publish(){
             PublishData();
            return Ok("Publish");
        }
        [HttpGet("Token")]
        public IActionResult GetToken()
        {

            return Ok( new { access_token = Token() });

        }


        string Token()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var alg = SecurityAlgorithms.HmacSha256;

            var login = new { Username = "pramesh", Role = "Admin" };

            var token = new JwtSecurityToken(

                issuer: configuration["jwt:issuer"],
                audience: configuration["jwt:audience"],
                claims: new[]
                {
                      new Claim(ClaimTypes.Name,login.Username),
                      new Claim(ClaimTypes.Role,login.Role),
                      new Claim(ClaimTypes.DateOfBirth,DateTime.Now.ToString()),
                      new Claim(JwtRegisteredClaimNames.Sub,"1")
                },
                signingCredentials: new SigningCredentials(key, alg),
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["jwt:expirey"]))

                );
            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            return access_token;
        }
        
        void Consume(){
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            ConsumeRabbit.Consume(channel);
        }

         void PublishData(){
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            ConsumeRabbit.Publish(channel);
        }


        [HttpGet("All"),Authorize(Roles ="Admin")]
        public IActionResult GetUser()
        {

            var users = user.GetUsers();
            return Ok(users);
        }

        [HttpGet("{Id}"),Authorize]
        public IActionResult ById(int Id)
        {
            var users = user.GetUsers().Where(p => p.Id == Id).FirstOrDefault();
            return Ok(users);
        }

        [HttpPost("SaveUser")]
        public IActionResult Save([FromBody] UserViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            user.PostUser(model);
            return Ok();
        }

        [HttpPost("EditUser")]
        public IActionResult Edit([FromBody] UserViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            user.EditUser(model);
            return Ok();
        }
    }
}
