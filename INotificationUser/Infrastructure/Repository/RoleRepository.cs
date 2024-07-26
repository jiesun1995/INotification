using INotificationUser.Domain.RoleAggregate.Entity;
using INotificationUser.Domain.RoleAggregate.Repository;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace INotificationUser.Infrastructure.Repository
{
    public class RoleRepository : EfCoreRepository<INotificationUserDBContext, Role, Guid>, IRoleRepository
    {
        public RoleRepository(IDbContextProvider<INotificationUserDBContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Role>> WithDetailsAsync()
        {
            var query = await base.WithDetailsAsync();
            query = query.Include(x => x.UserRoles);
            return query;
        }
    }
}
