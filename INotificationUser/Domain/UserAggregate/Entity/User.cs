using INotificationUser.Domain.RoleAggregate.Entity;
using Masuit.Tools;
using Masuit.Tools.Security;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace INotificationUser.Domain.UserAggregate.Entity
{
    public class User : FullAuditedAggregateRoot<Guid>
    {
        protected User() { }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; private set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? PhoneNumber { get; private set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? PassWord { get; private set; }
        /// <summary>
        /// 6位随机字符串，加密用到
        /// </summary>
        public string? Salt { get; private set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginNumber { get; private set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; private set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; private set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; private set; }

        public IReadOnlyCollection<UserRole> UserRoles=> _userRoles.AsReadOnly();
        private readonly List<UserRole> _userRoles = new List<UserRole>();


        public User(string? phonenumber,string? password)
        {
            Salt = Guid.NewGuid().ToString();
            if (!phonenumber.MatchPhoneNumber())
                throw new UserException("请输入正确的手机号码");
            if (string.IsNullOrWhiteSpace(password))
                throw new UserException("用户密码不能为空");
            PhoneNumber = phonenumber;
            PassWord = password.AESEncrypt(Salt);
        }
        public User(string? phonenumber,string? password, string? name, string? avatar, DateTime? birthday)
        {
            if (!phonenumber.MatchPhoneNumber())
                throw new UserException("请输入正确的手机号码");
            if (string.IsNullOrWhiteSpace(name))
                throw new UserException("用户名称不能为空");
            if (string.IsNullOrWhiteSpace(password))
                throw new UserException("用户密码不能为空");
            Salt = Guid.NewGuid().ToString();
            Name = name ?? Name;
            Birthday = birthday ?? birthday;
            Avatar = avatar ?? avatar;
            PhoneNumber = phonenumber;
            PassWord= password.AESEncrypt(Salt);
        }
        public void Edit(string? name, string? avatar, DateTime? birthday)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new UserException("用户名称不能为空");
            Name = name ?? Name;
            Birthday = birthday ?? birthday;
            Avatar = avatar ?? avatar;
        }

        public void Login(string? passWord)
        {
            var pwd = passWord.AESEncrypt(Salt);
            if (!string.IsNullOrWhiteSpace(PassWord) && pwd == PassWord)
            {
                LoginNumber++;
                LastLoginTime = DateTime.Now;
                return;
            }
            throw new UserException("密码不正确");
        }
        public void AddRole(Role role)
        {
            if (_userRoles.Any(x => x.RoleId == role.Id))
                return;
            _userRoles.Add(new UserRole(Id, role.Id));
        }
        public void AssignRoles(List<Role> roles)
        {
            var roleids = roles.Select(x => x.Id).ToList();
            foreach (var role in roles)
                AddRole(role);
            var deleroles = _userRoles.Where(x => !roleids.Contains(x.RoleId)).ToList();
            foreach (var role in deleroles)
                _userRoles.Remove(role);
        }
    }
}
