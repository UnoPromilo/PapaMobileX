namespace PapaMobileX.App.BusinessLogic.Builders.Interfaces;

public interface IHttpClientBuilder
{
    HttpClient JokeHttpClient { get; }
    HttpClient MainHttpClient { get; }

    public Uri? MainHttpClientBaseAddress { get; set; }
}