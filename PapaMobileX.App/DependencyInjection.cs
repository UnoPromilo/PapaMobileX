using System.Reflection;
using Microsoft.Extensions.Configuration;
using PapaMobileX.App.BusinessLogic.Builders.Concrete;
using PapaMobileX.App.BusinessLogic.HubClients.Abstraction;
using PapaMobileX.App.BusinessLogic.HubClients.Concrete;
using PapaMobileX.App.BusinessLogic.Mappers.Abstraction;
using PapaMobileX.App.BusinessLogic.Services.Concrete;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels;
using PapaMobileX.App.Foundation.Concrete;
using PapaMobileX.App.Foundation.Interfaces;
using PapaMobileX.App.Services.Concrete;
using PapaMobileX.App.Shared;
using PapaMobileX.App.Views;
using IHttpClientBuilder = PapaMobileX.App.BusinessLogic.Builders.Interfaces.IHttpClientBuilder;

namespace PapaMobileX.App;

public static class DependencyInjection
{
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<SteeringPage>();
        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<SteeringViewModel>();
        return builder;
    }

    public static MauiAppBuilder RegisterHubClients(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IHubClient, VideoHubClient>();
        return builder;
    }

    public static MauiAppBuilder RegisterMappers(this MauiAppBuilder builder)
    {
        Assembly assembly = typeof(BaseMapper<,>).Assembly;

        List<Type> mappers =
            assembly.GetTypes()
                    .Where(x =>
                               !x.IsAbstract &&
                               x.IsClass &&
                               x.Name.EndsWith("Mapper"))
                    .ToList();

        foreach (Type mapperType in mappers)
        {
            Type interfaceType = mapperType
                                 .GetInterfaces()
                                 .First(t => t.IsGenericType);
            builder.Services.AddTransient(interfaceType, mapperType);
        }

        return builder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddHttpClient(SharedConstants.JokeHttpClient,
                                       httpClient =>
                                       {
                                           httpClient.BaseAddress =
                                               new Uri(builder.Configuration.GetValue<string>(Constants
                                                                                                  .JokeServiceAddressKey));
                                       });

        builder.Services.AddHttpClient(SharedConstants.MainHttpClient);
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IOrientationService, OrientationService>();
        builder.Services.AddSingleton<IRandomJokeService, RandomJokeService>();
        builder.Services.AddSingleton<IHttpClientBuilder, HttpClientBuilder>();
        builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
        builder.Services.AddSingleton<ITokenService, TokenService>();
        builder.Services.AddSingleton<IVideoService, VideoService>();
        builder.Services.AddSingleton<ISteeringService, RotationService>();

        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IApiClientService, ApiClientService>();
        builder.Services.AddScoped<ILoginDataService, LoginDataService>();
        builder.Services.AddScoped<ILogoutService, LogoutService>();
        builder.Services.AddScoped<ISignalRConnectionService, SignalRConnectionService>();
        builder.Services.AddScoped<ISignalRSender, SignalRSender>();

        return builder;
    }
}