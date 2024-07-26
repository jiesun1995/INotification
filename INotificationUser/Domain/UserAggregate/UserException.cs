using INotificationUser.Domain.Base;

namespace INotificationUser.Domain.UserAggregate
{
    public class UserException: CommException
    {
        public UserException() { }
        public UserException(string message) : base(message) { }

        public UserException(string message, Exception? innerException) : base(message, innerException) { }

    }
}
