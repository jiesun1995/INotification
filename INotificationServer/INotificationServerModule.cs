using INotificationServer.Controllers;
using INotificationServer.Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.AspNetCore;
using Quartz.Impl.AdoJobStore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace INotificationServer
{
    [DependsOn(
        typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class INotificationServerModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var quartzDBConnectionString = context.Services.GetConfiguration().GetConnectionString("QuartzDB")!;
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options
                    .ConventionalControllers
                    .Create(typeof(INotificationServerModule).Assembly);
            });
            Configure<AbpAntiForgeryOptions>(options =>
            {
                options.AutoValidate = false;
            });
            context.Services.AddSwaggerGen(
               options =>
               {
                   options.HideAbpEndpoints();
                   options.SwaggerDoc("v1", new OpenApiInfo { Title = "WanZai API", Version = "v1" });
                   options.DocInclusionPredicate((docName, description) => true);
                   //var curr = AppContext.BaseDirectory;
                   //options.IncludeXmlComments($"{curr}/WanZaiShop.{nameof(WebApi)}.xml", true);
                   options.CustomSchemaIds(type => type.FullName);
                   options.EnableAnnotations();
                   //options.SchemaFilter<CustomSchemaFilters>();
                   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                       Name = "Authorization",
                       In = ParameterLocation.Header,
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = "Bearer"
                   });
                   options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,

                                },
                                new List<string>()
                            }
                   });
               });

            context.Services.AddAbpDbContext<QuartzDBContext>(options =>
            {
                options.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            context.Services.AddQuartz(options =>
            {
                options.SchedulerName = "INotification";
                options.UsePersistentStore(c =>
                {
                    c.UseSqlServer(quartzDBConnectionString);
                    c.Properties.Add("quartz.jobStore.tablePrefix", "[quartz].QRTZ_");
                    c.PerformSchemaValidation = true;
                    c.UseNewtonsoftJsonSerializer();
                });
            });

            context.Services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = false;
            });
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            if (env.IsDevelopment())
            {
                app.UseAbpExceptionHandling();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WanZai API");
                });
            }

            //app.UseAbpErrorHandling();
            //app.UseCors(CORS_POLICY);
            //app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuditing();
            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseConfiguredEndpoints();
        }
    }
}
