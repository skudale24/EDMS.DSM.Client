﻿using EDMS.DSM.Client.Managers.User;
using System.Net;

namespace EDMS.DSM.Client.Authentication.HttpUtils;

public class HttpInterceptorService
{
    private readonly CustomAuthenticationStateProvider _authState;
    private readonly HttpClientInterceptor _interceptor;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<HttpInterceptorService> _logger;
    private readonly ISnackbar _snackbar;
    private readonly IUserManager _userManager;

    public HttpInterceptorService(HttpClientInterceptor interceptor,
        ILocalStorageService localStorage,
        CustomAuthenticationStateProvider authState,
        IUserManager userManager, ISnackbar snackbar, ILogger<HttpInterceptorService> logger)
    {
        _interceptor = interceptor;
        _localStorage = localStorage;
        _userManager = userManager;
        _authState = authState;
        _snackbar = snackbar;
        _logger = logger;
    }

    /// <summary>
    ///     Wires custom event handler to outgoing HTTP requests.
    /// </summary>
    public void RegisterEvent()
    {
        _interceptor.BeforeSendAsync += InterceptRequestAsync;
    }

    private async Task InterceptRequestAsync(object? sender, HttpClientInterceptorEventArgs e)
    {
        var userToken = await _localStorage.GetItemAsStringAsync(StorageConstants.UserToken).ConfigureAwait(false);

        var info = GridExtensions.GetUserInfoFromToken(userToken);

        _logger.LogInformation($"{info.AspnetUserId}|{info.ProgramId}|{info.Expires}|{info.TimeOutMinutes}");
        _logger.LogInformation(DateTime.UtcNow.ToString());
        if (info != null)
        {
            _logger.LogInformation("Checking token validity...");
            if (info.Expires - DateTime.UtcNow < TimeSpan.FromMinutes(3)
                && info.Expires > DateTime.UtcNow)
            {
                _logger.LogInformation("Refreshing token...");
                RegenerateUserToken(info);
            }
        }
    }

    private void InterceptResponse(object? sender, HttpClientInterceptorEventArgs e)
    {
        if (e.Response.IsSuccessStatusCode)
        {
            return;
        }

        var statusCode = e.Response.StatusCode;

        switch (statusCode)
        {
            case HttpStatusCode.NotFound:
                // Redirect to "404 Not Found" screen from here...
                break;

            case HttpStatusCode.Unauthorized:
                //RefreshUserToken();
                break;

            case HttpStatusCode.BadRequest:
                _ = _snackbar.Add(
                    "Something went wrong. Please check if the format of the data is proper and try again.",
                    Severity.Error);
                break;

            case HttpStatusCode.InternalServerError:
                _ = _snackbar.Add(
                    "Something went wrong. Please check if the format of the data is proper and try again.",
                    Severity.Error);
                break;
            case HttpStatusCode.MethodNotAllowed:
                _ = _snackbar.Add(
                    "Something went wrong. Please check if the format of the data is proper and try again.",
                    Severity.Error);
                break;
            case HttpStatusCode.Continue:
            case HttpStatusCode.SwitchingProtocols:
            case HttpStatusCode.Processing:
            case HttpStatusCode.EarlyHints:
            case HttpStatusCode.OK:
            case HttpStatusCode.Created:
            case HttpStatusCode.Accepted:
            case HttpStatusCode.NonAuthoritativeInformation:
            case HttpStatusCode.NoContent:
            case HttpStatusCode.ResetContent:
            case HttpStatusCode.PartialContent:
            case HttpStatusCode.MultiStatus:
            case HttpStatusCode.AlreadyReported:
            case HttpStatusCode.IMUsed:
            case HttpStatusCode.Ambiguous:
            case HttpStatusCode.Moved:
            case HttpStatusCode.Found:
            case HttpStatusCode.RedirectMethod:
            case HttpStatusCode.NotModified:
            case HttpStatusCode.UseProxy:
            case HttpStatusCode.Unused:
            case HttpStatusCode.RedirectKeepVerb:
            case HttpStatusCode.PermanentRedirect:
            case HttpStatusCode.PaymentRequired:
            case HttpStatusCode.Forbidden:
            case HttpStatusCode.NotAcceptable:
            case HttpStatusCode.ProxyAuthenticationRequired:
            case HttpStatusCode.RequestTimeout:
            case HttpStatusCode.Conflict:
            case HttpStatusCode.Gone:
            case HttpStatusCode.LengthRequired:
            case HttpStatusCode.PreconditionFailed:
            case HttpStatusCode.RequestEntityTooLarge:
            case HttpStatusCode.RequestUriTooLong:
            case HttpStatusCode.UnsupportedMediaType:
            case HttpStatusCode.RequestedRangeNotSatisfiable:
            case HttpStatusCode.ExpectationFailed:
            case HttpStatusCode.MisdirectedRequest:
            case HttpStatusCode.UnprocessableEntity:
            case HttpStatusCode.Locked:
            case HttpStatusCode.FailedDependency:
            case HttpStatusCode.UpgradeRequired:
            case HttpStatusCode.PreconditionRequired:
            case HttpStatusCode.TooManyRequests:
            case HttpStatusCode.RequestHeaderFieldsTooLarge:
            case HttpStatusCode.UnavailableForLegalReasons:
            case HttpStatusCode.NotImplemented:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.ServiceUnavailable:
            case HttpStatusCode.GatewayTimeout:
            case HttpStatusCode.HttpVersionNotSupported:
            case HttpStatusCode.VariantAlsoNegotiates:
            case HttpStatusCode.InsufficientStorage:
            case HttpStatusCode.LoopDetected:
            case HttpStatusCode.NotExtended:
            case HttpStatusCode.NetworkAuthenticationRequired:
            default:
                _ = _snackbar.Add(
                    "Something went wrong. Please check if the format of the data is proper and try again.",
                    Severity.Error);
                // Redirect to "500 Internal Server Error" screen from here...
                break;
        }
    }

    private async void RegenerateUserToken(UserInfoDto userInfoDto)
    {
        _interceptor.BeforeSendAsync -= InterceptRequestAsync;

        var res = await _userManager.RegenerateUserTokenAsync(userInfoDto).ConfigureAwait(false);

        _logger.LogInformation($"Regenerated usertoken {res.userToken}");

        await _localStorage.SetItemAsStringAsync(StorageConstants.UserToken, res.userToken).ConfigureAwait(false);

        var info = GridExtensions.GetUserInfoFromToken(res.userToken);
        _logger.LogInformation($"{info.AspnetUserId}|{info.ProgramId}|{info.Expires}|{info.TimeOutMinutes}");
        _logger.LogInformation(DateTime.UtcNow.ToString());

        _interceptor.BeforeSendAsync += InterceptRequestAsync;
    }

    private async void RefreshUserToken(string aspnetuserId)
    {
        _interceptor.BeforeSendAsync -= InterceptRequestAsync;

        _logger.LogInformation($"Sending aspnetuserId...{aspnetuserId}");

        if (!string.IsNullOrEmpty(aspnetuserId))
        {
            var res = await _userManager.RefreshUserTokenAsync(aspnetuserId).ConfigureAwait(false);
            // _logger.LogInformation($"Refresh token response ...{res}");
            // _logger.LogInformation($"usertoken {res.userToken}");
            // _logger.LogInformation($"refresh token {res.refreshToken.token}");

            await _localStorage.SetItemAsStringAsync(StorageConstants.UserToken, res.userToken).ConfigureAwait(false);
            await _localStorage.SetItemAsStringAsync(StorageConstants.RefreshToken, res.refreshToken.token)
                .ConfigureAwait(false);
            _authState.UpdateAuthenticationState(res);
        }

        _interceptor.BeforeSendAsync += InterceptRequestAsync;
    }

    public void DisposeEvent()
    {
        _interceptor.BeforeSendAsync -= InterceptRequestAsync;
    }
}
