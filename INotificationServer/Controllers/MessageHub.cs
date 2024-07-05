using Microsoft.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.SignalR;

namespace INotificationServer.Controllers
{
    public class MessageHub: AbpHub
    {
        private readonly ILogger<MessageHub> _logger;

        public MessageHub(ILogger<MessageHub> logger)
        {
            _logger = logger;
        }

        public async Task SendMessage(string message)
        {
            _logger.LogInformation(message);
            await Clients.Caller.SendAsync("ReceiveMessage", $"{message}");
            var user = CurrentUser;
        }
    }
}
