using INotificationServer.Contracts;
using INotificationServer.Contracts.Dtos.Jobs;
using INotificationServer.Infrastructure.Jobs;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime;
using Volo.Abp.Application.Services;

namespace INotificationServer.Services
{
    public class QuartzManagerService: ApplicationService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<QuartzManagerService> _logger;

        public QuartzManagerService(ISchedulerFactory schedulerFactory, ILogger<QuartzManagerService> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        public async Task<IJobDetail> Add(CreateOrUpdateJob model) 
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            if (await scheduler.CheckExists(new JobKey(model.Name!)))
                throw new Exception("Job Already Exists");

            var jobType = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IJob).IsAssignableFrom(t))
                .FirstOrDefault(x => x.FullName == model.JobType);
            if (jobType == null)
                throw new Exception("Job Not Found");

            var jobDetail = JobBuilder.Create(jobType)
                .WithIdentity(model.Name!)
                .WithDescription(model.Description)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(model.Name!)
                .WithCronSchedule(model.CronExpression!)
                .Build();
            jobDetail.JobDataMap.Add(JobConstant.MESSAGE_RESPONSE, model.ToastData!);
            await scheduler.ScheduleJob(jobDetail,trigger);

            return jobDetail;
        }

        public async Task<IJobDetail> Test()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            if (await scheduler.CheckExists(new JobKey("test")))
                throw new Exception("Job Already Exists");

            var jobDetail = JobBuilder.Create<HubJob>()
                .WithIdentity("test")
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("test")
                .WithSimpleSchedule()
                .StartNow()
                .Build();
            await scheduler.ScheduleJob(jobDetail, trigger);

            return jobDetail;
        }
        //public async Task<IJobDetail> Update(CreateOrUpdateJob model)
        //{
        //    var scheduler = await _schedulerFactory.GetScheduler();
        //    if (!await scheduler.CheckExists(new JobKey(model.Name!)))
        //        throw new Exception("Job Not Found");

        //    var jobType = Assembly.GetExecutingAssembly().GetTypes()
        //        .Where(t => t.IsClass && !t.IsAbstract && typeof(IJob).IsAssignableFrom(t))
        //        .FirstOrDefault(x => x.FullName == model.JobType);
        //    if (jobType == null)
        //        throw new Exception("Job Not Found");

        //    var jobDetail = JobBuilder.Create(jobType);
        //}

        public async Task<bool> Delete(string name,string? groupName)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            JobKey jobKey;
            if (string.IsNullOrEmpty(groupName))
                jobKey= new JobKey(name);
            else
                jobKey = new JobKey(name, groupName);
            if (!await scheduler.CheckExists(jobKey))
                throw new Exception("Job Not Found");
            await scheduler.DeleteJob(jobKey);
            return true;
        }

        public async Task<IEnumerable<JobOutput>> GetAll()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var groupNames = await scheduler.GetJobGroupNames();
            var list = new List<JobOutput>();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                var jobKeys =await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
                foreach (var jobKey in jobKeys)
                {
                    var jobDetail = await scheduler.GetJobDetail(jobKey);
                    var trigger = (await scheduler.GetTriggersOfJob(jobKey)).AsEnumerable().FirstOrDefault();
                    var interval = string.Empty;
                    if (trigger is SimpleTriggerImpl)
                        interval = (trigger as SimpleTriggerImpl)?.RepeatInterval.ToString();
                    else
                        interval = (trigger as CronTriggerImpl)?.CronExpressionString;

                    list.Add(new JobOutput()
                    {
                        Name = jobKey.Name,
                        //LastErrMsg = jobDetail.JobDataMap.GetString(Constant.EXCEPTION),
                        //TriggerAddress = triggerAddress,
                        TriggerState = await scheduler.GetTriggerState(trigger!.Key),
                        PreviousFireTime = trigger.GetPreviousFireTimeUtc()?.LocalDateTime,
                        NextFireTime = trigger.GetNextFireTimeUtc()?.LocalDateTime,
                        BeginTime = trigger.StartTimeUtc.LocalDateTime,
                        Interval = interval,
                        EndTime = trigger.EndTimeUtc?.LocalDateTime,
                        Description = jobDetail!.Description,
                        //RequestType = jobDetail.JobDataMap.GetString(Constant.REQUESTTYPE),
                        //RunNumber = jobDetail.JobDataMap.GetLong(Constant.RUNNUMBER),
                        JobType = jobDetail.JobType.Name
                        //(triggers as SimpleTriggerImpl)?.TimesTriggered
                        //CronTriggerImpl 中没有 TimesTriggered 所以自己RUNNUMBER记录
                    });

                }
            }
            return list;
        }

    }
}
