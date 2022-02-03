using System.Collections.Immutable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using voonex.api.DTOs;
using voonex.api.Models;
using voonex.api.Services;

namespace voonex.api.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ServerController : ControllerBase
{
    private readonly IDbContextFactory<VoonexDbContext> _dbContextFactory;
    private readonly UserInfo _userInfo;

    public ServerController(IDbContextFactory<VoonexDbContext> dbContextFactory, UserInfo userInfo)
    {
        _dbContextFactory = dbContextFactory;
        _userInfo = userInfo;
    }

    [Authorize]
    [HttpPost("[action]")]
    [ProducesResponseType(typeof(CreateServerResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateServer(CreateServerRequest serverRequest)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var server = new Server()
        {
            Name = serverRequest.ServerName,
            ServerOwnerId = _userInfo.UserId,
        };
        server.ServerMembers.Add(new ServerMember()
        {
            UserId = _userInfo.UserId
        });

        await dbContext.Set<Server>().AddAsync(server);
        await dbContext.SaveChangesAsync();

        return Ok(new CreateServerResponse()
        {
            ServerId = server.Id
        });
    }

    [Authorize]
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(IEnumerable<GetUserServersResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserServers()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var servers = await dbContext.Set<ServerMember>().AsQueryable()
            .Where(x => x.UserId == _userInfo.UserId)
            .Select(x => new GetUserServersResponse()
            {
                ServerId = x.ServerId,
                ServerName = x.Server.Name
            }).ToListAsync();

        return Ok(servers);
    }
}