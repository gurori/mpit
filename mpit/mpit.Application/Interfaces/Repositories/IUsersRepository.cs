using mpit.mpit.Core.DTOs.User;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.Application.Interfaces.Repositories;

public interface IUsersRepository : IRepository<User>
{
    public Task<bool> TryCreateAsync(string login, string passwordHash, string role);
    public Task<UserEntity?> GetEntityByLoginAsync(string login);
    public Task<string> GetLoginByIdAsync(Guid id);
}
