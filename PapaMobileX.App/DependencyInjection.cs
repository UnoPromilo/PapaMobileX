using System.Reflection;
using Microsoft.Extensions.Configuration;
using PapaMobileX.App.BusinessLogic.Builders.Concrete;
using PapaMobileX.App.BusinessLogic.Mappers.Abstraction;
using PapaMobileX.App.BusinessLogic.Services.Concrete;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels;
using PapaMobileX.App.Foundation.Concrete;
using PapaMobileX.App.Foundation.Services;
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
        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginViewModel>();
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

        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IApiClientService, ApiClientService>();

        return builder;
    }
}