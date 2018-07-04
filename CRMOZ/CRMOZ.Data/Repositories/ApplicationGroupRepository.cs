using CRMOZ.Data.Infrastructure;
using CRMOZ.Model.Models;
using System.Collections.Generic;
using System.Linq;
namespace CRMOZ.Data.Repositories
{
    public interface IApplicationGroupRepository : IRepository<ApplicationGroup>
    {
        IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId);
        IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId);
        IEnumerable<ApplicationGroup> GetAllPaging(int page, int pageSize, out int totalRow, string filter);
    }

    public class ApplicationGroupRepository : RepositoryBase<ApplicationGroup>, IApplicationGroupRepository
    {
        IApplicationGroupRepository _appGroupRepository;
        public ApplicationGroupRepository(IDbFactory dbFactory, IApplicationGroupRepository appGroupRepository) : base(dbFactory)
        {
            this._appGroupRepository = appGroupRepository;
        }

        public IEnumerable<ApplicationGroup> GetAllPaging(int page, int pageSize, out int totalRow, string filter)
        {
            var query = _appGroupRepository.GetMulti(p => !p.IsDelete);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));

            totalRow = query.Count();
            return query.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
        }

        public IEnumerable<ApplicationGroup> GetListGroupByUserId(string userId)
        {
            var query = from g in DbContext.ApplicationGroups
                        join ug in DbContext.ApplicationUserGroups
                        on g.ID equals ug.GroupId
                        where ug.UserId == userId
                        select g;
            return query;
        }

        public IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId)
        {
            var query = from g in DbContext.ApplicationGroups
                        join ug in DbContext.ApplicationUserGroups
                        on g.ID equals ug.GroupId
                        join u in DbContext.Users
                        on ug.UserId equals u.Id
                        where ug.GroupId == groupId
                        select u;
            return query;
        }
    }
}