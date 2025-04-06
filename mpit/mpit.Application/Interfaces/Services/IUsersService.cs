namespace mpit.mpit.Application.Interfaces.Services;

public interface IUsersService
{
    public Task<string> LoginAsync(string login, string password, string role);
    public Task RegisterAsync();
}
