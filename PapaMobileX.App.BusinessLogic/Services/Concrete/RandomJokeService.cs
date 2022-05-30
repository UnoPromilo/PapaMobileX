using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using PapaMobileX.App.BusinessLogic.Builders.Interfaces;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class RandomJokeService : IRandomJokeService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public RandomJokeService(IHttpClientBuilder httpClientBuilder, IConfiguration configuration)
    {
        _httpClient = httpClientBuilder.JokeHttpClient;
        _configuration = configuration;
    }

    public async Task<string> GetRandomJoke()
    {
        var response = await _httpClient.GetAsync(_configuration.GetValue<string>(Constants.JokeServicePathKey));
        var readAsString = await response.Content.ReadAsStringAsync();
        return RemoveSpecialCharacters(readAsString);
    }
    
    private static string RemoveSpecialCharacters(string str)
    {
        str = Regex.Replace(str, @"[""\n]+", "", RegexOptions.Compiled);
        return Regex.Unescape(str);
    }
}