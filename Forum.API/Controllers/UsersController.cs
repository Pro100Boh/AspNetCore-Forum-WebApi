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
using Forum.BLL.Infrastructure;
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

                // jwt config
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and token to store client side
                return Ok(new
                {
                    user.UserId,
                    user.Username,
                    user.RegistrationDate,
                    user.Role,
                    Token = tokenString
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException ex)
            {
                return BadRequest(ex.Message);
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
            try
            {
                // mapping
                var userDTO = mapper.Map<RegistrationView, UserDTO>(registrationView);

                // save 
                userService.Create(userDTO, registrationView.Password);
                userService.SaveChanges();

                return Ok();
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, new { ex.Message, ex.InnerException, ex.StackTrace });
            }
        }

        //[HttpGet("{userId}/givemoder")]

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                // only allow admins to access other user records
                int currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(Role.Admin) && !User.IsInRole(Role.Moder))
                {
                    return Forbid("You can view only your acc data!!");
                }

                var user = userService.GetById(id);

                return Ok(user);
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (NotFoundInDbException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserUpdateView userUpdateView)
        {
            try
            {
                // mapping
                var userDTO = mapper.Map<UserUpdateView, UserDTO>(userUpdateView);

                userDTO.UserId = id;

                userService.Update(userDTO, userUpdateView.Password);
                userService.SaveChanges();

                return Ok();
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (NotFoundInDbException ex)
            {
                return BadRequest(ex.Message);
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
            try
            {
                userService.Delete(id);
                userService.SaveChanges();

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundInDbException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Internal Server Error
                return StatusCode(500, ex.Message);
            }
        }
    }
}