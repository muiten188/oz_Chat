using CRMOZ.Data.Infrastructure;
using CRMOZ.Model.Models;

namespace CRMOZ.Data.Repositories
{
    public interface IApplicationUserRoleRepository : IRepository<ApplicationUserRoles>
    {
    }

    public class ApplicationUserRoleRepository : RepositoryBase<ApplicationUserRoles>, IApplicationUserRoleRepository
    {
        public ApplicationUserRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}