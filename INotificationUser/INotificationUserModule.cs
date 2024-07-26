using INotificationUser.Contracts;
using INotificationUser.Domain.Base;
using INotificationUser.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace INotificationUser
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpBlobStoringFileSystemModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class INotificationUserModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAntiForgeryOptions>(options =>
            {
                options.AutoValidate = false;
            });
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options
                    .ConventionalControllers
                    .Create(typeof(INotificationUserModule).Assembly);
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
            context.Services.AddAbpDbContext<INotificationUserDBContext>(options =>
            {
                options.AddDefaultRepositories(true);
            });
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure<MyFileContainer>(configuration =>
                {
                    configuration.UseFileSystem(fileSystem =>
                    {
                        var dir = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        fileSystem.BasePath = dir;
                    });
                });

            });
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
            var key = Encoding.ASCII.GetBytes(AuthorizationConstant.JWT_SECRET_KEY);
            context.Services.AddAuthentication(config =>
            {
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
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
