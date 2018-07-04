using CRMOZ.Data.Infrastructure;
using CRMOZ.Model.Models;
using System.Collections.Generic;
using System.Linq;
namespace CRMOZ.Data.Repositories
{
    public interface IApplicationRoleRepository : IRepository<ApplicationRole>
    {
        IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId);
        IEnumerable<ApplicationRole> GetAllPaging(int page, int pageSize, out int totalRow, string filter);
    }

    public class ApplicationRoleRepository : RepositoryBase<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ApplicationRole> GetAllPaging(int page, int pageSize, out int totalRow, string filter)
        {
            var query = this.GetMulti(p => !p.IsDelete);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            totalRow = query.Count();
            return query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
        }

        public IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId)
        {
            var query = from g in DbContext.ApplicationRoles
                        join ug in DbContext.ApplicationRoleGroups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId
                        select g;
            return query;
        }
    }
}