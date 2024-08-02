using Microsoft.Extensions.Primitives;

namespace INotificationWeb.Infrastructure
{
    public class Api
    {
        public static class Task
        {
            public static string GetAll(string baseUri)=>$"{baseUri}";
        }
    }
}
