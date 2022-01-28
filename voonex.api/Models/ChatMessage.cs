using System.ComponentModel.DataAnnotations;

namespace voonex.api.Models;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ServerId { get; set; }
    public Server Server { get; set; } = null!;
    [StringLength(2000)]
    public string Content { get; set; } = null!;
}