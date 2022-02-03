namespace voonex.api.DTOs;

public class GetUserServersResponse
{
    public Guid ServerId { get; set; }
    public string ServerName { get; set; } = null!;
}