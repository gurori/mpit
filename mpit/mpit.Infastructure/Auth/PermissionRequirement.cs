using Microsoft.AspNetCore.Authorization;

namespace mpit.mpit.Infastructure.Auth
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
