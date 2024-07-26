using INotificationUser.Domain.UserAggregate.Entity;
using Volo.Abp.Domain.Entities.Auditing;

namespace INotificationUser.Domain.RoleAggregate.Entity
{
    public class Role : FullAuditedAggregateRoot<Guid>
    {
        protected Role() { }
        public string? Name { get; private set; }

        public string? NormalizedName { get; private set; }


        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
        private readonly List<UserRole> _userRoles = new List<UserRole>();

        public Role(string? name, string? normalizedName)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new RoleException("名称不能为空");
            Name = name;
            NormalizedName = normalizedName;
        }

        public void Edit(string? name, string? normalizedName)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new RoleException("名称不能为空");
            Name = name;
            NormalizedName = normalizedName;
        }
    }
}
