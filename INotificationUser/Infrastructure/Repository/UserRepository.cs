using INotificationUser.Domain.UserAggregate.Entity;
using INotificationUser.Domain.UserAggregate.Repository;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace INotificationUser.Infrastructure.Repository
{
    public class UserRepository : EfCoreRepository<INotificationUserDBContext, User, Guid>, IUserRepository
    {
        public UserRepository(IDbContextProvider<INotificationUserDBContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<User>> WithDetailsAsync()
        {
            var query = await base.WithDetailsAsync();

            query = query.Include(x => x.UserRoles);
            return query;

        }
    }
}
