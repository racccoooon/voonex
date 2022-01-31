using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace voonex.web.Services;

public class LoginManager : ILoginManager
{
    public async Task RefreshLoginStateAsync()
    {
        await _refreshFunction.Invoke();
    }

    private Func<Task> _refreshFunction = null!;
    public Func<Task> RefreshFunction
    {
        set => _refreshFunction = value;
    }
}

public interface ILoginManager
{
    Task RefreshLoginStateAsync();
    
    Func<Task> RefreshFunction { set; }
}