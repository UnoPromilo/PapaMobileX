using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace PapaMobileX.Server.Mappers;

public static class MappersConfiguration
{
    public static IServiceCollection ConfigureMappers(this IServiceCollection services)
    {
        Assembly assembly = typeof(MappersConfiguration).Assembly;

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
            services.AddTransient(interfaceType, mapperType);
        }

        return services;
    }
}