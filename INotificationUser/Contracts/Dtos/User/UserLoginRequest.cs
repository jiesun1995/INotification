using INotificationUser.Domain.Base;

namespace INotificationUser.Contracts.Dtos.User
{
    public class UserLoginRequest: BaseRequest
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? PassWord { get; set; }
    }
}
