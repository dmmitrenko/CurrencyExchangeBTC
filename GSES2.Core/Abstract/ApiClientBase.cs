using GSES2.Domain.Exceptions;
using Newtonsoft.Json;

namespace GSES2.Core.Abstract;
public abstract class ApiClientBase
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _serializerSettings;

    protected ApiClientBase(HttpClient httpClient, JsonSerializerSettings serializerSettings = default)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        if (string.IsNullOrEmpty(httpClient.BaseAddress.ToString()))
            throw new ArgumentNullException($"{nameof(httpClient.BaseAddress)}");

        _serializerSettings = serializerSettings ?? new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };
    }

    protected async Task<TResponse> GetAsync<TResponse>(string uri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(responseContent, _serializerSettings);
            return responseObject ?? throw new ArgumentNullException(nameof(responseObject), "The response object is null.");
        }

        throw new DomainException($"Request failed with message: {responseContent}", (int)response.StatusCode);
    }

    protected string BuildRequestUrl(string endpoint, Dictionary<string, string> parameters)
    {
        UriBuilder uriBuilder = new UriBuilder(_httpClient.BaseAddress);
        uriBuilder.Path = endpoint;

        var queryParameters = new List<string>();
        foreach (var parameter in parameters)
        {
            queryParameters.Add($"{Uri.EscapeDataString(parameter.Key)}={Uri.EscapeDataString(parameter.Value)}");
        }

        uriBuilder.Query = string.Join("&", queryParameters);

        return uriBuilder.ToString();
    }
}
