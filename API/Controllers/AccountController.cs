using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public DataContext Context { get; }
        public ITokenService TokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            TokenService = tokenService;
            this.Context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await Context.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }
            if(user.IsPasswordMatched(loginDto.Password) == false)
            {
                return Unauthorized("Invalid Password");
            }
            return new UserDto
            {
                UserName = user.UserName,
                Token = TokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName.ToLower()))
            {
                return BadRequest("Username is taken");
            }
            using (var hmac = new HMACSHA512())
            {
                var user = new AppUser
                {
                    UserName = registerDto.UserName.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PaswordSalt = hmac.Key
                };
                Context.Users.Add(user);
                await Context.SaveChangesAsync();

                return new UserDto {
                    UserName = user.UserName,
                    Token = TokenService.CreateToken(user)
                };
            }
        }
        private async Task<bool> UserExists(string userName)
        {
            return await Context.Users.AnyAsync(x => x.UserName == userName);
        }

    }
}