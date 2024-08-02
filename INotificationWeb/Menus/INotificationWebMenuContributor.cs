using Volo.Abp.UI.Navigation;

namespace INotificationWeb.Menus
{
    public class INotificationWebMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            await Task.CompletedTask;
            context.Menu.Items.Insert(0, new ApplicationMenuItem("home", "首页", "~/", icon: "fas fa-home", order: 0));
            context.Menu.Items.Insert(1, new ApplicationMenuItem("Tip", "任务提醒", "~/Task", icon: "fa-solid fa-clipboard-list", order: 0));
        }
    }
}
