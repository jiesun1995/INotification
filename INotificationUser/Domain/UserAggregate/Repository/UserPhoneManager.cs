using INotificationUser.Contracts;
using INotificationUser.Domain.RoleAggregate.Repository;
using INotificationUser.Domain.UserAggregate.Entity;
using INotificationUser.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Domain.Services;
using Volo.Abp.Security.Claims;

namespace INotificationUser.Domain.UserAggregate.Repository
{
    public class UserPhoneManager:DomainService
    {
        protected readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserPhoneManager(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> Create(string? phonenumber, string? passWord)
        {
            var user = (await _userRepository.FindAsync(x => x.PhoneNumber == phonenumber,false));
            if(user == null)
            {
                user = new User(phonenumber,passWord);
                await _userRepository.InsertAsync(user);
            }
            return user;
        }
        public async Task<User> Edit(Guid id,string? name, string? avatar, DateTime? birthday)
        {
            var user = (await _userRepository.GetAsync(x => x.Id == id))?? throw new UserException("未找到当前用户");
            user.Edit(name, avatar, birthday);
            user = await _userRepository.UpdateAsync(user);
            return user;
        }
        public async Task<string> Login(string? phonenumber,string? passWord)
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
