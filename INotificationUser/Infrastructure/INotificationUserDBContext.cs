using INotificationUser.Domain.RoleAggregate.Entity;
using INotificationUser.Domain.UserAggregate.Entity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace INotificationUser.Infrastructure
{
    [ConnectionStringName("QuartzDB")]
    public class INotificationUserDBContext : AbpDbContext<INotificationUserDBContext>
    {
        public INotificationUserDBContext(DbContextOptions<INotificationUserDBContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
