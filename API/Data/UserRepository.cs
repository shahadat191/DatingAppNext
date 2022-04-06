using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper Mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            Mapper = mapper;
        }


        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<MemberDto> GetMemberByUsernameAsync(string userName)
        {
            return await _context.Users
                        .Where(x => x.UserName == userName)
                        .ProjectTo<MemberDto>(Mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<MemberDto>> GetMembersAsync() => await _context.Users
                .Include(user => user.Photos)
                .ProjectTo<MemberDto>(Mapper.ConfigurationProvider).ToListAsync();

        public async Task<IEnumerable<AppUser>> GetUsersAsync() => await _context.Users.Include(p => p.Photos).ToListAsync();

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}