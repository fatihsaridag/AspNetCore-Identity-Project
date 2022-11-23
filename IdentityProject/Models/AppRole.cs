using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Models
{
    public class AppRole : IdentityRole
    {
        public string RoleType { get; set; }
    }
}
