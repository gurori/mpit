namespace mpit.mpit.DataAccess.Entities;

public class HelpEntity : Entity {
    public IList<string> UserLogins { get; set; } = [];
}