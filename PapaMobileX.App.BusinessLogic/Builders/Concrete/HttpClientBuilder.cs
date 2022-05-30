using PapaMobileX.App.BusinessLogic.Builders.Interfaces;
using PapaMobileX.App.Shared;

namespace PapaMobileX.App.BusinessLogic.Builders.Concrete;

public class HttpClientBuilder : IHttpClientBuilder
{
    private readonly IHttpClientFactory _clientFactory;

    public HttpClientBuilder(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public HttpClient JokeHttpClient => _clientFactory.CreateClient(SharedConstants.JokeHttpClient);

    public HttpClient MainHttpClient
    {
        get
        {
            HttpClient client = _clientFactory.CreateClient(SharedConstants.MainHttpClient);
            client.BaseAddress = MainHttpClientBaseAddress;
            return client;
        }
    }

    public Uri? MainHttpClientBaseAddress { get; set; }
}