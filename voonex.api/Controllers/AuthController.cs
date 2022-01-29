using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Randomizer;
using voonex.api.DTOs;
using voonex.api.Models;

namespace voonex.api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDbContextFactory<VoonexDbContext> _dbContextFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IDbContextFactory<VoonexDbContext> dbContextFactory, IHttpContextAccessor httpContextAccessor)
    {
        _dbContextFactory = dbContextFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        if (await dbContext.Set<User>().AsQueryable().AnyAsync(x => x.Email == registerRequest.Email))
        {
            return Conflict();
        }

        await dbContext.Set<User>().AddAsync(new User()
        {
            Email = registerRequest.Email,
            Name = registerRequest.UserName,
            Password = registerRequest.Password,
            Salt = "",
        });

        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var user = await dbContext.Set<User>().AsQueryable()
            .FirstOrDefaultAsync(x => x.Email == loginRequest.Email);
        if (user == null) return Forbid();
        if (user.Password != loginRequest.Password) return Forbid();
        var session = new Session()
        {
            Token = new RandomAlphanumericStringGenerator().GenerateValue(64),
            UserId = user.Id,
        };
        await dbContext.Set<Session>().AddAsync(session);
        return Ok(new LoginResponse()
        {
            Token = session.Token,
            UserId = user.Id,
        });
    }
    
    // [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [HttpPost("[action]")]
    public async Task<IActionResult> Logout()
    {
        return Ok();
    }
}