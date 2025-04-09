using AutoMapper;
using mpit.mpit.Application.Interfaces.Auth;
using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.Application.Interfaces.Services;
using mpit.mpit.Core.DTOs.User;
using mpit.mpit.Core.Exceptions;

namespace mpit.mpit.Application.Services;

public sealed class UsersService(
    IUsersRepository repository,
    IPasswordHasher passwordHasher,
    IMapper mapper,
    IJwtProvider jwtProvider
) : IUsersService
{
    private readonly IUsersRepository _repository = repository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IMapper _mapper = mapper;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task AddInfoAsync(string token, InfoRequest infoRequest)
    {
        Guid userId = await _jwtProvider.GetUserIdAsync(token);
        await _repository.AddInfoAsync(
            userId,
            infoRequest.Name,
            infoRequest.Date,
            infoRequest.Need
        );
    }

    public async Task<string> LoginAsync(string login, string password)
    {
        var userEntity =
            await _repository.GetEntityByLoginAsync(login)
            ?? throw new NotFoundException("Пользователь с данным логином не зарегистрирован");

        if (!_passwordHasher.Verify(password, userEntity.PasswordHash))
            throw new ConflictException("Неверный пароль");

        var user = _mapper.Map<User>(userEntity);
        var token = await _jwtProvider.GenerateTokenAsync(user.Id, user.Role);

        return token;
    }

    public async Task RegisterAsync(string login, string password, string role)
    {
        string hashedPassword = _passwordHasher.Generate(password);
        bool isCreated = await _repository.TryCreateAsync(login, hashedPassword, role);
        System.Console.WriteLine($"is generated - {isCreated}");
        // TODO: Login();
        if (!isCreated)
            throw new ConflictException("Данный пользователь уже зарегистрирован");
    }
}
