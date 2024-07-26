using Quartz;

namespace INotificationServer.Infrastructure.Jobs
{
    public class HelloJob : IJob
    {
        private readonly ILogger<HelloJob> _logger;

        public HelloJob(ILogger<HelloJob> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.CompletedTask;
            _logger.LogInformation($"{DateTime.Now} Hello Quartz!");
        }
    }
}
