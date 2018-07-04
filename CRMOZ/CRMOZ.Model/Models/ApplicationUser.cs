using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRMOZ.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Ten user hoac cong ty
        [Column(TypeName = "nvarchar")]
        [MaxLength(256)]
        public string FullName { set; get; }

        [MaxLength(256)]
        [Column(TypeName = "nvarchar")]
        public string Avartar { set; get; }

        [StringLength(128)]
        [Column(TypeName = "nvarchar")]
        public string CommonPassword { set; get; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Avatar", this.Avartar));
            userIdentity.AddClaim(new Claim("Fullname", this.FullName));
            return userIdentity;
        }
    }
}