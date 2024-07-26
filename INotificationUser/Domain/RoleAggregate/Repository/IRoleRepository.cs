using INotificationUser.Domain.RoleAggregate.Entity;
using INotificationUser.Domain.UserAggregate.Entity;
using Volo.Abp.Domain.Repositories;

namespace INotificationUser.Domain.RoleAggregate.Repository
{
    public interface IRoleRepository: IRepository<Role, Guid>
    {
    }
}
