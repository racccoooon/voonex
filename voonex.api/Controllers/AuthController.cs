using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Randomizer;
using voonex.api.DTOs;
using voonex.api.Models;
using voonex.api.Services;

namespace voonex.api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public const string ClaimTypeSessionToken = nameof(ClaimTypeSessionToken);
    
    private readonly IDbContextFactory<VoonexDbContext> _dbContextFactory;
    private readonly UserInfo _userInfo;
    
    public AuthController(IDbContextFactory<VoonexDbContext> dbContextFactory, UserInfo userInfo)
    {
        _dbContextFactory = dbContextFactory;
        _userInfo = userInfo;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
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

        await dbContext.SaveChangesAsync();
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypeSessionToken, session.Token),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        
        return Ok(new LoginResponse()
        {
            Token = session.Token,
            UserId = user.Id,
        });
    }
    
    [Authorize]
    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var sessionToken = _userInfo.SessionToken;
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var session = await dbContext.Set<Session>().AsQueryable()
            .FirstOrDefaultAsync(x => x.Token == sessionToken);

        if (session == null)
        {
            return Forbid();
        }

        dbContext.Set<Session>().Remove(session);
        
        await dbContext.SaveChangesAsync();

        await HttpContext.SignOutAsync();
        return Ok();
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(IsLoggedInResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> IsLoggedIn()
    {
        if (_userInfo.HasSessionToken == false)
        {
            return Ok(new IsLoggedInResponse()
            {
                IsLoggedIn = false
            });
        }
        
        var sessionToken = _userInfo.SessionToken;
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var session = await dbContext.Set<Session>().AsQueryable()
            .FirstOrDefaultAsync(x => x.Token == sessionToken);
        
        return Ok(new IsLoggedInResponse()
        {
            IsLoggedIn = session != null
        });
    }
}
