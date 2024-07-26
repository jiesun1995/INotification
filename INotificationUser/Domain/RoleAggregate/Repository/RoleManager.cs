using INotificationUser.Domain.RoleAggregate.Entity;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace INotificationUser.Domain.RoleAggregate.Repository
{
    public class RoleManager:DomainService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleManager(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> Create(string? name, string? normalizedName)
        {
            if (await _roleRepository.AnyAsync(x => name == x.Name))
                throw new RoleException($"已经存在角色{name}");

            var role = new Role(name, normalizedName);
            await _roleRepository.InsertAsync(role);
            return role;
        }

        public async Task<Role> Edit(Guid id, string? name, string? normalizedName)
        {
            if (await _roleRepository.AnyAsync(x => x.Id != id && name == x.Name))
                throw new RoleException($"已经存在角色{name}");

            var role = await _roleRepository.GetAsync(x => x.Id == id);
            role.Edit(name, normalizedName);
            role = await _roleRepository.UpdateAsync(role);
            return role;
        }


    }
}
