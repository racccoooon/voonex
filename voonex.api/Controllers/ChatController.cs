using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using voonex.api.DTOs;
using voonex.api.Models;
using voonex.api.Services;
using voonex.signalr.Hubs;

namespace voonex.api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ChatController : ControllerBase
{
    private readonly IDbContextFactory<VoonexDbContext> _dbContextFactory;
    private readonly UserInfo _userInfo;
    private readonly IHubContext<ChatHub, IChatHub> _chatHubContext;

    public ChatController(IDbContextFactory<VoonexDbContext> dbContextFactory, UserInfo userInfo, IHubContext<ChatHub, IChatHub> chatHubContext)
    {
        _dbContextFactory = dbContextFactory;
        _userInfo = userInfo;
        _chatHubContext = chatHubContext;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SendChatMessage(SendMessageRequest sendMessageRequest)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var message = new ChatMessage()
        {
            UserId = _userInfo.UserId,
            Content = sendMessageRequest.Message,
            ServerId = sendMessageRequest.ServerId
        };
        
        await dbContext.Set<ChatMessage>().AddAsync(message);
        await dbContext.SaveChangesAsync();

        await _chatHubContext.Clients
            .Groups(sendMessageRequest.ServerId.ToString())
            .ChatUpdated(new ChatUpdateDto(sendMessageRequest.Message, _userInfo.Name));
        
        return Ok();
    }

    [Authorize]
    [HttpGet("{serverId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<GetChatMessagesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChatMessages(Guid serverId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var chatMessages = await dbContext.Set<ChatMessage>().AsQueryable()
            .Where(x => x.ServerId == serverId)
            .Select(x => new GetChatMessagesResponse
            {
                Content = x.Content,
                SenderName = x.User.Name,
            })
            .ToListAsync();
        return Ok(chatMessages);
    }
}