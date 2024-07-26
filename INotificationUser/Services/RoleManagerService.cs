using INotificationUser.Contracts.Dtos.Role;
using INotificationUser.Contracts.Dtos.User;
using INotificationUser.Domain.Base;
using INotificationUser.Domain.RoleAggregate.Repository;
using INotificationUser.Domain.UserAggregate.Entity;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using INotificationUser.Domain.RoleAggregate.Entity;
using Microsoft.AspNetCore.Authorization;

namespace INotificationUser.Services
{
    [Authorize(Roles = "admin")]
    public class RoleManagerService:ApplicationService
    {
        private readonly RoleManager _roleManager;
        private readonly IRoleRepository _roleRepository;

        public RoleManagerService(RoleManager roleManager, IRoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }

        public async Task<BaseResponse> Create(CreateOrUpdateRoleRequest request)
        {
            await _roleManager.Create(request.Name, request.NormalizedName);
            return new BaseResponse { Data = true };
        }

        public async Task<BaseResponse> Update(Guid id, CreateOrUpdateRoleRequest request)
        {
            var user = await _roleManager.Edit(id, request.Name, request.NormalizedName);
            return new BaseResponse { Data = true };
        }

        public async Task<BaseResponse> GetList(PagedAndSortedResultRequest request)
        {
            var query = await _roleRepository.GetQueryableAsync();
            query = query.OrderBy(request.Sorting ?? $"{nameof(User.CreationTime)}");
            var count = await query.CountAsync();
            var list = await query.Page(request.Page, request.Rows).ToListAsync();
            return new BaseResponse
            {
                Data = new PagedResultDto<Role>
                {
                    TotalCount = count,
                    Items = list
                }
            };
        }
        public async Task<BaseResponse> Get(Guid id)
        {
            var user = await _roleRepository.GetAsync(id);
            return new BaseResponse { Data = user };
        }
    }
}
