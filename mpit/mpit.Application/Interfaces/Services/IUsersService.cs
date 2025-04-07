namespace mpit.mpit.Application.Interfaces.Services;

public interface IUsersService
{
    public Task<string> LoginAsync(string login, string password);
    public Task RegisterAsync(string login, string password, string role);
}
