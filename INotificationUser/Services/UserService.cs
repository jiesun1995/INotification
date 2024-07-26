using INotificationUser.Contracts.Dtos.User;
using INotificationUser.Domain.Base;
using INotificationUser.Domain.UserAggregate.Entity;
using INotificationUser.Domain.UserAggregate.Repository;
using Volo.Abp.Application.Services;

namespace INotificationUser.Services
{
    public class UserService: ApplicationService
    {
        private readonly UserPhoneManager _userPhoneManager;

        public UserService(UserPhoneManager userPhoneManager)
        {
            _userPhoneManager = userPhoneManager;
        }

        public async Task<BaseResponse> Create(CreateUserPhoneRequest request)
        {
            await _userPhoneManager.Create(request.PhoneNumber, request.PassWord);
            return new BaseResponse { Data = true };
        }

        public async Task<BaseResponse> Update(UserPhoneRequest request)
        {
            var user = await _userPhoneManager.Edit(CurrentUser.Id!.Value, request.Name, request.Avatar, request.Birthday);
            return new BaseResponse { Data = user };
        }

        public async Task<string> Login(UserLoginRequest request)
        {
            var token = await _userPhoneManager.Login(request.PhoneNumber, request.PassWord);
            return token;
        }
    }
}
