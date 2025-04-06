namespace mpit.mpit.DataAccess.Entities;

public class UserEntity : Entity
{
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
