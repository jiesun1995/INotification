using Volo.Abp.Domain.Entities;

namespace INotificationUser.Domain.UserAggregate.Entity
{
    public class UserRole:Entity<int>
    {
        protected UserRole() { }
        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; private set; }
        public Guid RoleId { get; private set; }
    }
}
