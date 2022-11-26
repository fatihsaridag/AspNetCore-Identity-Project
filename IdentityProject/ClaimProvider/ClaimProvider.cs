using IdentityProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityProject.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                AppUser user = await _userManager.FindByNameAsync(identity.Name);
                if (user != null)
                {
                    if (user.BirthDay!=null)
                    {
                        var today = DateTime.Today;
                        var age = today.Year - user.BirthDay?.Year;
                        if (age > 18)
                        {
                            Claim licenseClaim = new Claim("license", true.ToString(), ClaimValueTypes.String, "Internal");
                            identity.AddClaim(licenseClaim);
                        }
                    }


                    if (user.City != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "City"))
                        {
                            Claim CityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");
                            identity.AddClaim(CityClaim);
                        }
                    }
                }  
            }   
             return principal; 
        }
    }
}
