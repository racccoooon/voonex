using System.ComponentModel.DataAnnotations;

namespace voonex.api.Models;

public class Server
{
    public Guid Id { get; set; }
    [StringLength(160)]
    public string Name { get; set; } = null!;
    public User ServerOwner { get; set; } = null!;
    public Guid ServerOwnerId { get; set; }
    public List<ServerMember> ServerMembers { get; set; } = new();
}