using INotificationUser.Contracts.Dtos.User;
using INotificationUser.Domain.Base;
using INotificationUser.Domain.UserAggregate.Entity;
using INotificationUser.Domain.UserAggregate.Repository;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using System.Linq.Dynamic.Core;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace INotificationUser.Services
{
    [Authorize(Roles = "admin")]
    public class UserManagerService:ApplicationService
    {
        private readonly UserManager _userManager;
        private readonly IUserRepository _userRepository;
        public UserManagerService(UserManager userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> Create(CreateOrUpdateUserRequest request)
        {
            var user = await _userManager.Create(request.PhoneNumber, request.PassWord, request.Name, request.Avatar, request.Birthday,request.RoleIds);
            return new BaseResponse { Data = true };
        }

        public async Task<BaseResponse> Update(Guid id, CreateOrUpdateUserRequest request)
        {
            var user = await _userManager.Edit(id, request.Name, request.Avatar, request.Birthday, request.RoleIds);
            return new BaseResponse { Data = true };
        }

        public async Task<BaseResponse> GetList(PagedAndSortedResultRequest request)
        {
            var query = await _userRepository.GetQueryableAsync();
            query = query.OrderBy(request.Sorting ?? $"{nameof(User.CreationTime)}");
            var count = await query.CountAsync();
            var list = await query.Page(request.Page, request.Rows).ToListAsync();
            return new BaseResponse
            {
                Data = new PagedResultDto<User>
                {
                    TotalCount = count,
                    Items = list
                }
            };
        }
        public async Task<BaseResponse> Get(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            return new BaseResponse { Data = user };
        }
    }
}
