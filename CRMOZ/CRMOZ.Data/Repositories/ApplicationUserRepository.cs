using CRMOZ.Data.Infrastructure;
using CRMOZ.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace CRMOZ.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationRole GetRoleDefault();

        IEnumerable<ApplicationUserRoles> GetUserRoles(string userId);
    }

    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        private OZChatDbContext db;

        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            db = new OZChatDbContext();
        }

        public ApplicationRole GetRoleDefault()
        {
            return DbContext.ApplicationRoles.FirstOrDefault(p => p.IsDefault && !p.IsDelete);
        }

        public IEnumerable<ApplicationUserRoles> GetUserRoles(string userId)
        {
            return DbContext.ApplicationUserRoles.Where(p => p.UserId == userId).ToList();
        }
    }
}