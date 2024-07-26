using INotificationUser.Domain.Base;

namespace INotificationUser.Contracts.Dtos.User
{
    public class CreateOrUpdateUserRequest: BaseRequest
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
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? PassWord { get; set; }

        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
