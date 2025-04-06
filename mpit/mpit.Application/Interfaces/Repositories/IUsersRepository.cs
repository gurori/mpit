using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.Application.Interfaces.Repositories;

public interface IUsersRepository : IRepository<UserEntity>
{
    public Task<bool> TryCreateAsync(string login, string passwordHash, string role);
}
