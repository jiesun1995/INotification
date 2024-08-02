using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace INotificationWeb
{
    [Dependency(ReplaceServices = true)]
    public class INotificationWebBrandingProvider: DefaultBrandingProvider
    {
        public override string AppName => "我的订阅";
        public override string? LogoUrl => "/favicon.ico";
    }
}
