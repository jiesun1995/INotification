using INotificationServer.Contracts;
using INotificationServer.Controllers;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Quartz;

namespace INotificationServer.Infrastructure.Jobs
{
    public class HubJob : IJob
    {
        private readonly ILogger<HubJob> _logger;
        private readonly IHubContext<MessageHub> _hubContext;


        public HubJob(ILogger<HubJob> logger, IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap.Get(JobConstant.MESSAGE_RESPONSE);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(data));
        }
    }
}
