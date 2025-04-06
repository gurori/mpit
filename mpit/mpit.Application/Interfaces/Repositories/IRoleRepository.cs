namespace mpit.mpit.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        public Task<HashSet<string>> GetPermissionsAsync(string roleName);
    }
}
