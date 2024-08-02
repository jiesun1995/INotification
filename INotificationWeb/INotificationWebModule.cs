using INotificationWeb.Menus;
using INotificationWeb.Services;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation;

namespace INotificationWeb
{
    [DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpSettingManagementWebModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpAspNetCoreSerilogModule)
    )]
    public class INotificationWebModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient<TaskService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Sample. Default lifetime is 2 minutes
                //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                ;
            //context.Services.Configure<AbpAspNetCoreMvcOptions>(options =>
            //{
            //    options
            //        .ConventionalControllers
            //        .Create(typeof(INotificationWebModule).Assembly);
            //});

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new INotificationWebMenuContributor());
            });

            //context.Services.ad

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
