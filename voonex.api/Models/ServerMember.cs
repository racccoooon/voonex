namespace voonex.api.Models;

public class ServerMember
{
    public Guid Id { get; set; }
    public Guid ServerId { get; set; }
    public Server Server { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}