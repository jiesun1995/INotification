using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.SqlServer;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace INotificationServer.Infrastructure
{
    [ConnectionStringName("QuartzDB")]
    public class QuartzDBContext: AbpDbContext<QuartzDBContext>
    {
        public QuartzDBContext(DbContextOptions<QuartzDBContext> options) : base(options)
        {
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddQuartz(builder => builder.UseSqlServer());
        }
    }
}
