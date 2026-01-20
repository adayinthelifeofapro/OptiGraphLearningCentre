using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using OptiGraphLearningCentre.Client.Models.Configuration;
using OptiGraphLearningCentre.Client.Models.Query;

namespace OptiGraphLearningCentre.Client.Services;

/// <summary>
/// GraphQL client implementation with Optimizely Graph authentication support
/// </summary>
public class GraphQLClient : IGraphQLClient
{
    private readonly HttpClient _httpClient;
    private readonly ISettingsService _settingsService;
    private readonly ILogger<GraphQLClient> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public LastRequestInfo? LastRequest { get; private set; }

    public GraphQLClient(
        HttpClient httpClient,
        ISettingsService settingsService,
        ILogger<GraphQLClient> logger)
    {
        _httpClient = httpClient;
        _settingsService = settingsService;
        _logger = logger;
    }

    public async Task<GraphQLResponse<JsonElement>> ExecuteAsync(
        GraphQLRequest request,
        CancellationToken cancellationToken = default)
    {
        var settings = await _settingsService.GetSettingsAsync();
        var stopwatch = Stopwatch.StartNew();

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, settings.Endpoint);
        var requestBody = JsonSerializer.Serialize(request, JsonOptions);
        httpRequest.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        // Apply authentication
        ApplyAuthentication(httpRequest, requestBody, settings);

        // Track request info
        LastRequest = new LastRequestInfo
        {
            Url = settings.Endpoint,
            Method = "POST",
            RequestHeaders = httpRequest.Headers
                .ToDictionary(h => h.Key, h => string.Join(", ", h.Value)),
            RequestBody = requestBody
        };

        try
        {
            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            stopwatch.Stop();

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            LastRequest.StatusCode = (int)response.StatusCode;
            LastRequest.ResponseHeaders = response.Headers
                .ToDictionary(h => h.Key, h => string.Join(", ", h.Value));
            LastRequest.ResponseBody = responseBody;
            LastRequest.Duration = stopwatch.Elapsed;

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GraphQL request failed with status {StatusCode}: {Body}",
                    response.StatusCode, responseBody);

                return new GraphQLResponse<JsonElement>
                {
                    Errors = new List<GraphQLError>
                    {
                        new() { Message = $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}" }
                    }
                };
            }

            var result = JsonSerializer.Deserialize<GraphQLResponse<JsonElement>>(
                responseBody, JsonOptions);

            return result ?? new GraphQLResponse<JsonElement>();
        }
        catch (HttpRequestException ex)
        {
            stopwatch.Stop();
            LastRequest.Duration = stopwatch.Elapsed;

            _logger.LogError(ex, "Failed to execute GraphQL request");

            return new GraphQLResponse<JsonElement>
            {
                Errors = new List<GraphQLError>
                {
                    new() { Message = $"Connection error: {ex.Message}" }
                }
            };
        }
    }

    public async Task<GraphQLResponse<T>> ExecuteAsync<T>(
        GraphQLRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync(request, cancellationToken);

        T? data = default;
        if (response.Data is { } dataElement && dataElement.ValueKind != JsonValueKind.Undefined)
        {
            data = dataElement.Deserialize<T>(JsonOptions);
        }

        return new GraphQLResponse<T>
        {
            Data = data,
            Errors = response.Errors,
            Extensions = response.Extensions
        };
    }

    public async Task<(bool Success, string Message)> TestConnectionAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var testQuery = new GraphQLRequest
            {
                Query = "{ __schema { queryType { name } } }"
            };

            var response = await ExecuteAsync(testQuery, cancellationToken);

            if (response.HasErrors)
            {
                var errorMessage = string.Join("; ", response.Errors!.Select(e => e.Message));
                return (false, $"Connection failed: {errorMessage}");
            }

            return (true, "Connection successful!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Connection test failed");
            return (false, $"Connection failed: {ex.Message}");
        }
    }

    private void ApplyAuthentication(HttpRequestMessage request, string body, GraphSettings settings)
    {
        switch (settings.AuthMode)
        {
            case AuthenticationMode.SingleKey when !string.IsNullOrEmpty(settings.SingleKey):
                request.Headers.TryAddWithoutValidation("Authorization", $"epi-single {settings.SingleKey}");
                break;

            case AuthenticationMode.Hmac when !string.IsNullOrEmpty(settings.AppKey) && !string.IsNullOrEmpty(settings.Secret):
                ApplyHmacAuthentication(request, body, settings);
                break;

            case AuthenticationMode.None:
            default:
                // No authentication
                break;
        }
    }

    private void ApplyHmacAuthentication(HttpRequestMessage request, string body, GraphSettings settings)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var nonce = Guid.NewGuid().ToString("N");

        // Create the string to sign: AppKey + Timestamp + Nonce + HTTP Method + Target + Body
        var method = "POST";
        var target = new Uri(settings.Endpoint).PathAndQuery;

        var stringToSign = $"{settings.AppKey}{timestamp}{nonce}{method}{target}{body}";

        // Compute HMAC-SHA256 signature
        var secretBytes = Encoding.UTF8.GetBytes(settings.Secret!);
        var dataBytes = Encoding.UTF8.GetBytes(stringToSign);

        using var hmac = new HMACSHA256(secretBytes);
        var hash = hmac.ComputeHash(dataBytes);
        var signature = Convert.ToBase64String(hash);

        // Set headers
        request.Headers.TryAddWithoutValidation("Authorization", $"epi-hmac {settings.AppKey}:{timestamp}:{nonce}:{signature}");
    }
}
