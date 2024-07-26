using Microsoft.AspNetCore.Mvc;
using Quartz;
using System.Reflection;
using Volo.Abp.Application.Services;

namespace INotificationServer.Services
{
    public class JobTypeService: ApplicationService
    {
        private readonly IServiceProvider _serviceProvider;

        public JobTypeService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Dictionary<string, string?>> GetList()
        {
            await Task.CompletedTask;
            var jobs = Assembly.GetExecutingAssembly().GetTypes()
                 .Where(t => t.IsClass && !t.IsAbstract && typeof(IJob).IsAssignableFrom(t))
                 .ToDictionary(x=>x.Name,x=>x.FullName);

            return jobs;
        }
        //[HttpGet]
        //public async Task<string> CreateJob(string jobFullName)
        //{
        //    var jobType = Assembly.GetExecutingAssembly().GetTypes()
        //        .Where(t => t.IsClass && !t.IsAbstract && typeof(IJob).IsAssignableFrom(t) )
        //        .FirstOrDefault(x=>x.FullName == jobFullName);
        //        if(jobType == null)
        //         throw new Exception("Job Not Found");
        //    var job = _serviceProvider.GetService(jobType);

        //    var jobBuilder = JobBuilder.Create(jobType)
        //        .WithIdentity(job.GetType().Name)
        //        .StoreDurably()
        //        ;

        //    return job.GetType().Name;
        //}


    }
}
