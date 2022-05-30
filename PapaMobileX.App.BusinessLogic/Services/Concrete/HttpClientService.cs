using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using PapaMobileX.App.BusinessLogic.Builders.Interfaces;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class HttpClientService : IHttpClientService
{
    private const string ContentTypeHeaderValue = "application/json";
    private readonly IHttpClientBuilder _httpClientBuilder;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly object _tokenUsageLock = new();
    private CancellationTokenSource _cancellationTokenSource = new();

    public HttpClientService(IHttpClientBuilder httpClientBuilder)
    {
        _httpClientBuilder = httpClientBuilder;
    }

    public async Task<Result<HttpError, T?>> GetAsync<T>(string resource, CancellationToken cancellationToken = default)
    {
        using Result<HttpError, HttpResponseMessage> response =
            await InternalSendAsync(HttpMethod.Get, resource, null, cancellationToken);

        if (response.IsFailed)
            return response.Error;

        return await DeserializeResponse<T>(response.Data);
    }

    public async Task<Result<HttpError, T?>> PostAsync<T>(string resource,
                                                          object body,
                                                          CancellationToken cancellationToken = default)
    {
        using Result<HttpError, HttpResponseMessage> response =
            await InternalSendAsync(HttpMethod.Post, resource, body, cancellationToken);

        if (response.IsFailed)
            return response.Error;

        return await DeserializeResponse<T>(response.Data);
    }

    public async Task<Result<HttpError>> PostAsync(string resource,
                                                   object body,
                                                   CancellationToken cancellationToken = default)
    {
        using Result<HttpError, HttpResponseMessage> response =
            await InternalSendAsync(HttpMethod.Post, resource, body, cancellationToken);

        return response.IsFailed ? response.Error : Result<HttpError>.Ok();
    }

    public void CancelAllRequests()
    {
        lock (_tokenUsageLock)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }

    private CancellationToken LinkCancellationToken(CancellationToken cancellationToken)
    {
        lock (_tokenUsageLock)
        {
            return CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken)
                                          .Token;
        }
    }

    private async Task<Result<HttpError, HttpResponseMessage>> InternalSendAsync(
        HttpMethod httpMethod,
        string resource,
        object? body,
        CancellationToken cancellationToken = default)
    {
        try
        {
            CancellationToken combinedCancellationToken = LinkCancellationToken(cancellationToken);
            HttpClient httpClient = _httpClientBuilder.MainHttpClient;

            UriBuilder uriBuilder = new(httpClient.BaseAddress!)
            {
                Path = resource
            };
            Uri fullUri = uriBuilder.Uri;

            var httpRequestMessage = new HttpRequestMessage(httpMethod, fullUri);

            if (body != null)
            {
                string serializeBody = JsonSerializer.Serialize(body);
                httpRequestMessage.Content = new StringContent(serializeBody, Encoding.UTF8, ContentTypeHeaderValue);
            }

            HttpResponseMessage responseMessage =
                await httpClient.SendAsync(httpRequestMessage, combinedCancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
                return HttpError.UnsuccessfulRequest(responseMessage.StatusCode);

            return responseMessage;
        }
        catch (WebException webException)
        {
            return HttpError.HttpRequestException(webException);
        }
        catch (SocketException webException)
        {
            return HttpError.HttpRequestException(webException);
        }
        catch (HttpRequestException hre)
        {
            return HttpError.HttpRequestException(hre);
        }
        catch (OperationCanceledException)
        {
            return HttpError.WasCancelled();
        }
    }

    private async Task<T?> DeserializeResponse<T>(HttpResponseMessage responseMessage,
                                                  bool ignoreJsonExceptions = false)
    {
        try
        {
            string stringResponse = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringResponse, _serializerOptions);
        }
        catch (JsonException) when (ignoreJsonExceptions)
        {
            return default;
        }
    }
}