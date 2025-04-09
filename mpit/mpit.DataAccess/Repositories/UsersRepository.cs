using AutoMapper;
using Microsoft.EntityFrameworkCore;
using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.Core.DTOs.User;
using mpit.mpit.DataAccess.DbContexts;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.DataAccess.Repositories;

public sealed class UsersRepository(ApplicationDbContext dbContext, IMapper mapper)
    : IUsersRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task AddInfoAsync(Guid userId, string name, string date, string need)
    {
        System.Console.WriteLine(userId);
        var user = await _dbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        System.Console.WriteLine(user?.Login);
        var infoEntity = new InfoEntity
        {
            Name = name,
            Date = date,
            Need = need,
            UserId = userId,
            User = user!,
        };
        user!.Info = infoEntity;
        await _dbContext.Infos.AddAsync(infoEntity);
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IList<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserEntity?> GetEntityByLoginAsync(string login)
    {
        var userEntity = await _dbContext
            .Users.AsNoTracking()
            .Where(u => u.Login == login)
            .FirstOrDefaultAsync();

        return userEntity;
    }

    public async Task<UserEntity[]> GetInfosByLogins(string[] logins)
    {
        var users = await _dbContext
            .Users.AsNoTracking()
            .Include(u => u.Info)
            .Where(u => logins.Contains(u.Login))
            .ToArrayAsync();

        //return users.Select(u => u.Info!).ToArray();
        return users;
    }

    public async Task<string> GetLoginByIdAsync(Guid id)
    {
        return await _dbContext
            .Users.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => u.Login)
            .FirstAsync();
    }

    public Task<IList<User>> GetManyByIdsAsync(IList<Guid> ids)
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
