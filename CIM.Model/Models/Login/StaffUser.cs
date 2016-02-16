using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CIM.Model.Models.Login
{
    public class StaffUser: CustomerUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<StaffUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Department { get; set; }
        public string EmployeeNumber { get; set; }
        
    }
}
