using mpit.mpit.Application.Interfaces.Auth;
using mpit.mpit.Application.Interfaces.Repositories;

namespace mpit.mpit.Infastructure.Auth
{
    public sealed class PermissionService(IRoleRepository roleRepository) : IPermissionService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<HashSet<string>> GetPermissionsAsync(string roleName)
        {
            return await _roleRepository.GetPermissionsAsync(roleName);
        }
    }
}
