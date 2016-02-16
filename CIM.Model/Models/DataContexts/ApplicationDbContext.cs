using CIM.Model.Models.Login;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CIM.Model.Models.DataContexts
{
    public class ApplicationDbContext : IdentityDbContext<CustomerUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
