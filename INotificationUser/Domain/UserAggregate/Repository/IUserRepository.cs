using INotificationUser.Domain.UserAggregate.Entity;
using Volo.Abp.Domain.Repositories;

namespace INotificationUser.Domain.UserAggregate.Repository
{
    public interface IUserRepository:IRepository<User,Guid>
    {
    }
}
