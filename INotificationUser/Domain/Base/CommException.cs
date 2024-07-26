namespace INotificationUser.Domain.Base
{
    public class CommException : Exception
    {
        public CommException() { }
        public CommException(string message) :base(message) { }

        public CommException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
