using INotificationServer.Contracts.Dtos.Jobs;
using INotificationServer.Contracts;
using Quartz;
using System.Reflection;
using Volo.Abp.Application.Services;
using INotificationServer.Contracts.Dtos.ToastJobs;
using INotificationServer.Infrastructure.Jobs;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;

namespace INotificationServer.Services
{
    public class ToastManagerJobService : ApplicationService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<ToastManagerJobService> _logger;
        const string _groupName = nameof(HubJob);

        public ToastManagerJobService(ISchedulerFactory schedulerFactory, ILogger<ToastManagerJobService> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }
        public async Task<JobOutput> Add(CreateToastJob model)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            if (await scheduler.CheckExists(new JobKey(model.Name!, _groupName)))
                throw new Exception("Job Already Exists");

            var jobDetail = JobBuilder.Create<HubJob>()
                .WithIdentity(model.Name!, _groupName)
                .WithDescription(model.Description)
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(model.Name!, _groupName)
                .WithCronSchedule(model.CronExpression!)
                .Build();
            jobDetail.JobDataMap.Add(JobConstant.MESSAGE_RESPONSE, model.ToastData!);
            await scheduler.ScheduleJob(jobDetail, trigger);
            var interval = string.Empty;
            if (trigger is SimpleTriggerImpl)
                interval = (trigger as SimpleTriggerImpl)?.RepeatInterval.ToString();
            else
                interval = (trigger as CronTriggerImpl)?.CronExpressionString;
            var  jobOutput = new JobOutput()
            {
                Name = jobDetail.Key.Name,
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
            };
            return jobOutput;
        }
        /// <summary>
        /// 查询全部Job
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<JobOutput>> GetAll()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var list = new List<JobOutput>();

            var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(_groupName));
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
            return list;
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<bool> TriggerJobAsync(string name)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            JobKey jobKey = new JobKey(name, _groupName);
            if (!await scheduler.CheckExists(jobKey))
                throw new Exception("Job Not Found");
            await scheduler.TriggerJob(jobKey);
            return true;
        }
        /// <summary>
        /// 删除Job
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(string name, string? groupName)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            JobKey jobKey;
            if (string.IsNullOrEmpty(groupName))
                jobKey = new JobKey(name);
            else
                jobKey = new JobKey(name, groupName);
            if (!await scheduler.CheckExists(jobKey))
                throw new Exception("Job Not Found");
            await scheduler.DeleteJob(jobKey);
            return true;
        }
    }
}
