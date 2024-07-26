using INotificationUser.Domain.RoleAggregate.Entity;
using INotificationUser.Domain.RoleAggregate.Repository;
using INotificationUser.Domain.RoleAggregate;
using Volo.Abp.Domain.Services;
using INotificationUser.Domain.UserAggregate.Entity;
using INotificationUser.Infrastructure.Repository;
using Volo.Abp.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Security.Claims;
using System.Data;
using INotificationUser.Contracts;

namespace INotificationUser.Domain.UserAggregate.Repository
{
    public class UserManager:DomainService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserManager(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> Create(string? phonenumber, string? password, string? name, string? avatar, DateTime? birthday, List<Guid> roleIds)
        {
            if (await _userRepository.AnyAsync(x => x.PhoneNumber == phonenumber))
                throw new RoleException($"手机号已注册");
            var user = new User(phonenumber, password, name, avatar, birthday);
            var roles = await _roleRepository.GetListAsync(x => roleIds.Contains(x.Id));
            user.AssignRoles(roles);
            user = await _userRepository.InsertAsync(user);
            return user;
        }

        public async Task<User> Edit(Guid id,  string? name, string? avatar, DateTime? birthday,List<Guid> roleIds)
        {
            var user = await _userRepository.GetAsync(id);
            user.Edit( name, avatar, birthday);
            var roles = await _roleRepository.GetListAsync(x => roleIds.Contains(x.Id));
            user.AssignRoles(roles);
            user = await _userRepository.UpdateAsync(user);
            return user;
        }
        public async Task<string> Login(string phonenumber, string passWord)
        {
            var user = (await _userRepository.GetAsync(x => x.PhoneNumber == phonenumber)) ?? throw new UserException("未找到用户");
            user.Login(passWord);
            var roles = await _roleRepository.GetListAsync(x => x.UserRoles.Any(y => y.UserId == user.Id));
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthorizationConstant.JWT_SECRET_KEY);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber!),
                new Claim(AbpClaimTypes.UserId, user.Id.ToString()),
                new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber!),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(AbpClaimTypes.Role, role.Name!));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
