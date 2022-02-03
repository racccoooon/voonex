namespace voonex.api.DTOs;

public class SendMessageRequest
{
    public string Message { get; set; } = null!;
    public Guid ServerId { get; set; }
}