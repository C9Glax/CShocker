using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CShocker.Devices.Additional;

public static class ApiHttpClient
{
    internal static HttpResponseMessage MakeAPICall(HttpMethod method, string uri, string? jsonContent, ILogger? logger = null, params ValueTuple<string, string>[] customHeaders)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        ProductInfoHeaderValue userAgent = new (fvi.ProductName ?? fvi.FileName, fvi.ProductVersion);
        HttpRequestMessage request = new (method, uri)
        {
            Headers =
            {
                UserAgent = { userAgent },
                Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
            }
        };
        if (jsonContent is not null && jsonContent.Length > 0)
            request.Content =
                new StringContent(jsonContent, Encoding.UTF8, new MediaTypeHeaderValue("application/json"));
        foreach ((string, string) customHeader in customHeaders)
            request.Headers.Add(customHeader.Item1, customHeader.Item2);
        logger?.Log(LogLevel.Debug, $"Request-URI: {request.RequestUri}\n\r" +
                                         $"Request-Headers: \n\t{string.Join("\n\t", request.Headers.Select(h => $"{h.Key} {string.Join(", ", h.Value)}"))}\n\r" +
                                         $"Request-Content: {request.Content?.ReadAsStringAsync().Result}");

        HttpClient httpClient = new();
        HttpResponseMessage response = httpClient.Send(request);
        logger?.Log(!response.IsSuccessStatusCode ? LogLevel.Error : LogLevel.Debug,
            $"{request.RequestUri} response: {response.StatusCode}");
        httpClient.Dispose();
        return response;
    }
}