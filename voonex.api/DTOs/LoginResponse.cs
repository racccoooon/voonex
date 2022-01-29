namespace voonex.api.DTOs;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public Guid UserId { get; set; }
}