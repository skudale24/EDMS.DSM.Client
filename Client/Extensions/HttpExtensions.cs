using System.Web;

namespace EDMS.DSM.Client.Extensions;

public static class HttpExtensions
{
    public static string GenerateQueryString<T>(T filter)
    {
        if (filter == null)
        {
            return string.Empty;
        }

        var properties = filter.GetType().GetProperties()
            .Where(w => !string.IsNullOrWhiteSpace(w.GetValue(filter, null)?.ToString()))
            .Select(s => $"{s.Name}={HttpUtility.UrlEncode(s.GetValue(filter, null)?.ToString())}")
            .ToList();

        var queryString = string.Join("&", properties);

        return queryString;
    }
}
