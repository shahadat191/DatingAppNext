using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PaswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public GenderType Gender {get; set;}
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Interests { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public AppUser() {
            Created = DateTime.UtcNow;
            LastActive = DateTime.UtcNow;
        }

    }

    public enum GenderType {
        Male,
        Female
    }
}