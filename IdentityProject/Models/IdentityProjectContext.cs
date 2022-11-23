using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityProject.Models
{
    public class IdentityProjectContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public IdentityProjectContext(DbContextOptions<IdentityProjectContext> options) : base(options)
        {
        }
    }
}
