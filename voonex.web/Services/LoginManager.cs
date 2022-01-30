using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace voonex.web.Services;

public class LoginManager : ILoginManager
{
    private readonly Func<Task> _refreshApp;

    public LoginManager(Func<Task> refreshApp)
    {
        _refreshApp = refreshApp;
    }

    public async Task RefreshLoginStateAsync()
    {
        await _refreshApp.Invoke();
    }
}

public interface ILoginManager
{
    Task RefreshLoginStateAsync();
}