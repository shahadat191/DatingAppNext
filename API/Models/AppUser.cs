using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PaswordSalt { get; set; }

        public bool IsPasswordMatched(string loginPassword) 
        {
            using var hmac = new HMACSHA512(PaswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginPassword));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != PasswordHash[i]) return false;
            }
            return true;
        }
    }
}