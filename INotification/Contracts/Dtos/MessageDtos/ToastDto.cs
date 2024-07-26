using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INotification.Contracts.Dtos.MessageDtos
{
    public class ToastDto
    {
        public string? Message { get; set; }
        public string? IconAddress { get; set; }
    }
}
