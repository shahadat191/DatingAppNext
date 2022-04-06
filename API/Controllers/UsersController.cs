using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository UserRepository;
        private readonly IMapper Mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            Mapper = mapper;
            UserRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto >>> GetUsers()
        {
            return Ok(await UserRepository.GetMembersAsync());
        }

        [HttpGet("{username}")]
        //[Authorize]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await UserRepository.GetMemberByUsernameAsync(username);
            return Ok(Mapper.Map<MemberDto>(user));
        }
    }
}