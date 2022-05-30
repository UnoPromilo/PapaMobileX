namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface IRandomJokeService
{
    Task<string> GetRandomJoke();
}