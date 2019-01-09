using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forum.API.Helpers;
using Forum.API.Models;
using Forum.BLL.DTO;
using Forum.BLL.Exceptions;
using Forum.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Forum.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService userService;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;

        public UsersController(
            IUsersService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticationView authenticationView)
        {
            try
            {
                var user = userService.Authenticate(authenticationView.Username, authenticationView.Password);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and token to store client side
                return Ok(new UserView
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    RegistrationDate = user.RegistrationDate,
                    Token = tokenString
                });
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (NotFoundInDbException notFoundEx)
            {
                return BadRequest(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegistrationView registrationView)
        {
            // mapping
            var userDTO = mapper.Map<RegistrationView, UserDTO>(registrationView);

            try
            {
                // save 
                userService.Create(userDTO, registrationView.Password);
                userService.SaveChanges();

                return Ok();
            }
            catch (ArgumentOutOfRangeException argOutOfRangeEx)
            {
                return BadRequest(argOutOfRangeEx.Message);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = userService.GetById(id);

                return Ok(user);
            }
            catch (ArgumentOutOfRangeException argOutOfRangeEx)
            {
                return BadRequest(argOutOfRangeEx.Message);
            }
            catch (NotFoundInDbException notFoundEx)
            {
                return BadRequest(notFoundEx.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]RegistrationView registrationView)
        {
            try
            {
                // mapping
                var userDTO = mapper.Map<RegistrationView, UserDTO>(registrationView);

                userDTO.UserId = id;

                // save 
                userService.Update(userDTO, registrationView.Password);
                userService.SaveChanges();

                return Ok();
            }
            catch (ArgumentNullException argNullEx)
            {
                return BadRequest(argNullEx.Message);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            userService.Delete(id);
            userService.SaveChanges();

            return Ok();
        }
    }
}