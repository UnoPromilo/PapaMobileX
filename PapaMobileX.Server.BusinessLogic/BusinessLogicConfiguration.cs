using Microsoft.Extensions.DependencyInjection;
using PapaMobileX.Server.BusinessLogic.Services.Concrete;
using PapaMobileX.Server.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.Server.BusinessLogic;

public static class BusinessLogicConfiguration
{
    public static IServiceCollection ConfigureBusinessLogic(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}