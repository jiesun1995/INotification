using Azure;
using System.Text.Json;

namespace INotificationUser.Domain.Base
{
    public class BaseResponse: BaseMessage
    {
        public BaseResponse()
        {
        }

        public BaseResponse(Guid id)
        {
            base._correlationId = id;
        }

        public int Code { get; set; } = 200;

        public object? Data { get; set; }


        public string? Message { get; set; }

        public static Response Return(Func<Response> func)
        {
            return func();
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
