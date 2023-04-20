namespace EDMS.DSM.Client.Managers.Common;

public class LookupManager : ILookupManager
{
    private readonly HttpRequest _httpRequest;

    public LookupManager(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    public async Task<bool> ValidatePincodeAsync(string pincode)
    {
        var urlWithParams = $"{PincodeEndPoints.IsValidPincode}/{pincode}";
        var result = await _httpRequest.GetRequestAsync<bool>(urlWithParams).ConfigureAwait(false);

        return result.Result;
    }
}
