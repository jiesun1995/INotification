using INotificationWeb.Infrastructure;
using INotificationWeb.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace INotificationWeb.Services
{
    public class TaskService : ApplicationService
    {
        private readonly HttpClient _apiClient;
        private readonly ILogger<TaskService> _logger;
        private readonly string _taskUrl;

        public TaskService(HttpClient apiClient, ILogger<TaskService> logger, IOptions<AppSettings> settings)
        {
            _apiClient = apiClient;
            _logger = logger;
            _taskUrl = $"{settings.Value.BaseUrl}/api/app/quartz-manager";
        }

        public async Task<PagedResultDto<TaskDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var url = Api.Task.GetAll(_taskUrl);
            _logger.LogDebug("[GetAll] -> Calling {Uri} to get the basket", url);
            var response = await _apiClient.GetAsync(url);
            _logger.LogDebug("[GetAll] -> response code {StatusCode}", response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();

            var list = string.IsNullOrEmpty(responseString) ?
            new List<TaskDto>() : JsonConvert.DeserializeObject<List<TaskDto>>(responseString!) ?? new List<TaskDto>();
            return new PagedResultDto<TaskDto>(list.Count, list);
        }
    }
}
