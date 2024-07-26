using INotificationServer.Contracts.Dtos.MessageDtos;
using System.ComponentModel.DataAnnotations;

namespace INotificationServer.Contracts.Dtos.ToastJobs
{
    public class CreateToastJob
    {
        [Required]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [Required]
        public string? CronExpression { get; set; }
        [Required]
        public ToastDto? ToastData { get; set; }
    }
}
