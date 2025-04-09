namespace mpit.mpit.DataAccess.Entities;

public sealed class InfoEntity : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Need { get; set; } = string.Empty;

    public Guid UserId { get; set; } // FK
    public UserEntity User { get; set; } = null!;
}
