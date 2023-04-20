using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace EDMS.DSM.Client.Extensions;

internal static class ResultExtensions
{
    internal static async Task<IListApiResult<T>?> ToResultAsync<T>(this HttpResponseMessage response)
    {
        try
        {
            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var responseObject = JsonSerializer.Deserialize<ListApiResult<T>>(responseAsString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.Preserve
                });
            return responseObject;
        }
        catch (Exception)
        {
            return null;
        }
    }

    internal static async Task<T?> ToExternalResultAsync<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var responseObject = JsonSerializer.Deserialize<T>(responseAsString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        return responseObject;
    }

    internal static async Task<T?> ToExternalResultFromXmlAsync<T>(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(responseAsString);
        var responseObject = (T)serializer.Deserialize(reader)!;

        return responseObject;
    }

    internal static async Task<IApiResult?> ToResultAsync(this HttpResponseMessage response)
    {
        var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var responseObject = JsonSerializer.Deserialize<ApiResult>(responseAsString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.Preserve
            });
        return responseObject;
    }
}
