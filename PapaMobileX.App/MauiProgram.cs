using System.Reflection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using PapaMobileX.App.Controls;
using PapaMobileX.App.Effects;
using PapaMobileX.App.Effects.Implementations;

namespace PapaMobileX.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole().AddDebug());
        builder
            .UseMauiApp<App>()
            .RegisterViews()
            .RegisterViewModels()
            .RegisterHubClients()
            .RegisterMappers()
            .RegisterServices()
            .UseMauiCommunityToolkit()
            .ConfigureEffects(effects => { effects.Add<TintImageEffect, TintImageEffectRouter>(); })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Black.ttf", "RobotoBlack");
                fonts.AddFont("Roboto-BlackItalic.ttf", "RobotoBlackItalic");
                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                fonts.AddFont("Roboto-BoldItalic.ttf", "RobotoBoldItalic");
                fonts.AddFont("Roboto-Italic.ttf", "RobotoItalic");
                fonts.AddFont("Roboto-Light.ttf", "RobotoLight");
                fonts.AddFont("Roboto-LightItalic.ttf", "RobotoLightItalic");
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                fonts.AddFont("Roboto-MediumItalic.ttf", "RobotoMediumItalic");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                fonts.AddFont("Roboto-Thin.ttf", "RobotoThin");
                fonts.AddFont("Roboto-ThinItalic.ttf", "RobotoThinItalic");
            })
            .ConfigureMauiHandlers(h =>
            {
                #if ANDROID
                    h.AddHandler(typeof(VideoImage), typeof(VideoImageHandler));
                #endif
                h.AddMauiControlsHandlers();
            });

        string? assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        using Stream? stream = Assembly
                               .GetExecutingAssembly()
                               .GetManifestResourceStream($"{assemblyName}.appsettings.json");

        IConfigurationRoot? config = new ConfigurationBuilder()
                                     .AddJsonStream(stream)
                                     .Build();

        builder.Configuration.AddConfiguration(config);

        return builder.Build();
    }
}