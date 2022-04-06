using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Extensions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if(await context.Users.AnyAsync())
                return;
            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach(var user in users)
            {
                user.UserName = user.UserName.ToLower();
                (user.PasswordHash, user.PaswordSalt) = user.GetPasswordInfo("Password");
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
            
        }
    }
}