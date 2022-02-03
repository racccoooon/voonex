using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voonex.api.DTOs;
using voonex.api.Models;
using voonex.api.Services;

namespace voonex.api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IDbContextFactory<VoonexDbContext> _dbContextFactory;
    private readonly UserInfo _userInfo;

    public ChatController(IDbContextFactory<VoonexDbContext> dbContextFactory, UserInfo userInfo)
    {
        _dbContextFactory = dbContextFactory;
        _userInfo = userInfo;
    }

    [Authorize]
    [HttpPost("[action]")]
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
        return Ok();
    }
}