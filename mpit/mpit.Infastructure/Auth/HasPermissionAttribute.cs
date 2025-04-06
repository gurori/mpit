using Microsoft.AspNetCore.Authorization;
using mpit.mpit.Core.Enums;

namespace mpit.mpit.Infastructure.Auth
{
    public sealed class HasPermissionAttribute(Permission permission)
        : AuthorizeAttribute(policy: permission.ToString()) { }
}
