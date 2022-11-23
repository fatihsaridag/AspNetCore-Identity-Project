using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityProject.Models
{
    public class AppUser : IdentityUser
    {
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Job { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Picture { get; set; }
        public int Gender { get; set; }
    }
}
