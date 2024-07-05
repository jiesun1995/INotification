using INotificationServer.Controllers;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace INotificationServer
{
    [DependsOn(
        typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class INotificationServerModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //Configure<AbpAspNetCoreMvcOptions>(options =>
            //{
            //    options
            //        .ConventionalControllers
            //        .Create(typeof(WanZaiShopApplicationModule).Assembly);
            //});
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
