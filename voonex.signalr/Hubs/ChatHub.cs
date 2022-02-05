using Microsoft.AspNetCore.SignalR;

namespace voonex.signalr.Hubs;

public record ChatUpdateDto(string Content, string UserName);

public interface IChatHub
{
    Task ChatUpdated(ChatUpdateDto chatUpdateDto);
}

public class ChatHub : Hub<IChatHub>
{
    public async Task JoinChat(Guid serverId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, serverId.ToString());
    }

    public async Task LeaveChat(Guid serverId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, serverId.ToString());
    }
}