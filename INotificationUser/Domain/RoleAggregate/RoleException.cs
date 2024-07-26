using INotificationUser.Domain.Base;

namespace INotificationUser.Domain.RoleAggregate
{
    public class RoleException: CommException
    {
        public RoleException() { }
        public RoleException(string message) : base(message) { }

        public RoleException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
