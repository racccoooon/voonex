using Microsoft.Extensions.Options;
using voonex.api.client;
using voonex.web.Settings;

namespace voonex.web.Services;

public class ClientFactory
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<VoonexApiSettings> _apiSettings;

    public ClientFactory(HttpClient httpClient, IOptions<VoonexApiSettings> apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public Client CreateClient()
    {
        return new Client(_apiSettings.Value.Url, _httpClient);
    }
}