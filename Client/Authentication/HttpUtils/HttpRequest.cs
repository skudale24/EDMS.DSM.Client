using System.Text.Json;

namespace EDMS.DSM.Client.Authentication.HttpUtils;

public class HttpRequest
{
    private readonly CustomAuthenticationStateProvider _authState;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navManager;
    private readonly IServiceProvider _serviceProvider;

    public HttpRequest(IServiceProvider serviceProvider,
        CustomAuthenticationStateProvider authState, ILocalStorageService localStorage,
        IHttpClientFactory httpClientFactory, NavigationManager navManager)
    {
        _serviceProvider = serviceProvider;
        _localStorage = localStorage;
        _httpClientFactory = httpClientFactory;
        _navManager = navManager;
        _authState = authState;
    }

    public async Task<IListApiResult<T>> GetRequestAsync<T>(string uri)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
        _ = httpClient.EnableIntercept(_serviceProvider);
        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";
        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }

        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);

        var response = await httpClient.GetAsync(absoluteUrl).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return await response.ToResultAsync<T>().ConfigureAwait(false);
    }

    public async Task<T> ExternalGetRequestAsync<T>(string uri)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        _ = httpClient.EnableIntercept(_serviceProvider);
        var response = await httpClient.GetAsync(uri).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
        return await response.ToExternalResultAsync<T>().ConfigureAwait(false);
    }

    public async Task<T> ExternalGetRequestFromXmlAsync<T>(string uri)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        _ = httpClient.EnableIntercept(_serviceProvider);
        var response = await httpClient.GetAsync(uri).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
        return await response.ToExternalResultFromXmlAsync<T>().ConfigureAwait(false);
    }

    public async Task<Stream> GetStreamAsync(string uri)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        _ = httpClient.EnableIntercept(_serviceProvider);
        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }

        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);


        var response = await httpClient.GetAsync(absoluteUrl).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        return stream;
    }

    public async Task<Stream?> GetStreamDataAsync<TIn>(string uri, TIn values)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();

            _ = httpClient.EnableIntercept(_serviceProvider);
            var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

            var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);

            var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

            if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
            {
                var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
                httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
            }

            httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
            httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);

            StringContent serialized = new(JsonSerializer.Serialize(values), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(absoluteUrl, serialized).ConfigureAwait(false);

            _ = response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return stream;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<IApiResult> PostRequest1Async<TIn>(string uri, TIn values)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        _ = httpClient.EnableIntercept(_serviceProvider);

        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);

        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);

        StringContent serialized = new(JsonSerializer.Serialize(values), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(absoluteUrl, serialized).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return await response.ToResultAsync().ConfigureAwait(false);
    }

    public async Task<IListApiResult<TOut>> PostRequestAsync<TIn, TOut>(string uri, TIn values)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        _ = httpClient.EnableIntercept(_serviceProvider);

        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }


        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);

        StringContent serialized = new(JsonSerializer.Serialize(values), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(absoluteUrl, serialized).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return await response.ToResultAsync<TOut>().ConfigureAwait(false);
    }

    public async Task<IListApiResult<TOut>> PutRequestAsync<TIn, TOut>(string uri, TIn values)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        _ = httpClient.EnableIntercept(_serviceProvider);

        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);


        StringContent serialized = new(JsonSerializer.Serialize(values), Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync(absoluteUrl, serialized).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        return await response.ToResultAsync<TOut>().ConfigureAwait(false);
    }

    public async Task<IApiResult> PostMultiPartRequestAsync(string uri, MemoryStream ms, string fileName)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        _ = httpClient.EnableIntercept(_serviceProvider);
        using MultipartFormDataContent content = new() { { new StreamContent(ms), "file", fileName } };

        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }

        httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);

        httpClient.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");

        var response = await httpClient.PostAsync(absoluteUrl, content).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
        return await response.ToResultAsync<ApiResult>().ConfigureAwait(false);
    }

    public async Task<IApiResult> DeleteRequestAsync(string uri)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        _ = httpClient.EnableIntercept(_serviceProvider);

        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }

        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);

        var response = await httpClient.DeleteAsync(absoluteUrl).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
        return await response.ToResultAsync<ApiResult>().ConfigureAwait(false);
    }


    public async Task<IApiResult<T>> GetRequestNewAsync<T>(string uri)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        _ = httpClient.EnableIntercept(_serviceProvider);
        var absoluteUrl = $"{EndPoints.ApiBaseUrl}/{uri}";

        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);
        var authState = await _authState.GetAuthenticationStateAsync().ConfigureAwait(false);

        if (authState.User.HasClaim(x => x.Type == ClaimTypes.UserData))
        {
            var aspnetuserId = authState.User.Claims.Single(x => x.Type == ClaimTypes.UserData).Value;
            httpClient.DefaultRequestHeaders.Add(StorageConstants.AspNetUserId, aspnetuserId);
        }

        httpClient.DefaultRequestHeaders.Add(AppConstants.AppTokenHeaderKey, AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add(AppConstants.UserTokenHeaderKey, userToken);

        var response = await httpClient.GetAsync(absoluteUrl).ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();

        return await response.ToResultAsync<T>().ConfigureAwait(false);
    }


    public async Task<TokenResult> AuthGetRequestAsync<T>(string uri)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        _ = httpClient.EnableIntercept(_serviceProvider);

        var refreshToken =
            await _localStorage.GetItemAsStringAsync(StorageConstants.RefreshToken).ConfigureAwait(false);

        var absoluteUrl = $"{EndPoints.LoginPage}/{uri}";

        httpClient.DefaultRequestHeaders.Add("appToken", AppConstants.AppTokenValue);
        httpClient.DefaultRequestHeaders.Add("reftoken", refreshToken);

        var response = await httpClient.GetAsync(absoluteUrl).ConfigureAwait(false);

        _ = response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var res1 = JsonSerializer.Deserialize<Root>(content);
            return res1.result;
        }

        _navManager.NavigateTo("/logout");

        return null;
    }
}
