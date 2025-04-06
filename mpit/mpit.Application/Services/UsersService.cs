using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.Application.Interfaces.Services;

namespace mpit.mpit.Application.Services;

public sealed class UsersService(IUsersRepository repository) : IUsersService
{
    private readonly IUsersRepository _repository = repository;

    public async Task<string> LoginAsync(string login, string password, string role)
    {
        //string passwordHash = ""; // use PasswordHasher

        bool isCreated = await _repository.TryCreateAsync(login, password, role);

        return ""; // use JwtProvider
    }

    public Task RegisterAsync()
    {
        throw new NotImplementedException();
    }
}
