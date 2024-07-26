using INotificationUser.Domain.Base;

namespace INotificationUser.Contracts.Dtos.User
{
    public class UserPhoneRequest : BaseRequest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }
    }
}
