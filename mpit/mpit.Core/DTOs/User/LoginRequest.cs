namespace mpit.mpit.Core.DTOs.User;

public sealed record LoginRequest(string Login, string Password, string Role);
