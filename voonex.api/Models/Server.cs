using System.ComponentModel.DataAnnotations;

namespace voonex.api.Models;

public class Server
{
    public Guid Id { get; set; }
    [StringLength(160)]
    public string Name { get; set; } = null!;
}