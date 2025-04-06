using Microsoft.EntityFrameworkCore;
using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.DataAccess.DbContexts;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.DataAccess.Repositories;

public sealed class UsersRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<UserEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserEntity?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<UserEntity>> GetManyByIdsAsync(IList<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> TryCreateAsync(string login, string passwordHash, string role)
    {
        var isAlreadyExist = await _dbContext.Users.AsNoTracking().AnyAsync(x => x.Login == login);

        if (isAlreadyExist)
            return false;

        var userEntity = new UserEntity
        {
            Login = login,
            PasswordHash = passwordHash,
            Role = role,
        };

        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
