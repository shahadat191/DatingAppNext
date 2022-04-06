using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Models;

namespace API.Extensions
{
    public static class AppUserExtension
    {
        public static bool IsPasswordMatched(this AppUser appUser, string loginPassword)
        {
            using var hmac = new HMACSHA512(appUser.PaswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginPassword));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != appUser.PasswordHash[i]) return false;
            }
            return true;
        }    
        public static Tuple<byte[],byte[]> GetPasswordInfo(this AppUser appUser,string password)
        {
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var paswordSalt = hmac.Key;
            return Tuple.Create(passwordHash, paswordSalt);
        }    
    }
}