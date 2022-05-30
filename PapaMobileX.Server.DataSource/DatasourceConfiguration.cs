using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PapaMobileX.Server.DataSource;

public static class DatasourceConfiguration
{
    public static IServiceCollection ConfigureDatasource(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<CommonContext>(
                                         options =>
                                             options.UseSqlite(configuration.GetConnectionString(Constants
                                                                                                     .DefaultConnection)
                                                              ));

        Assembly assembly = typeof(DatasourceConfiguration).Assembly;

        List<Type> repositories =
            assembly.GetTypes()
                    .Where(x =>
                               !x.IsAbstract &&
                               x.IsClass &&
                               x.Name.EndsWith("Repository"))
                    .ToList();

        foreach (Type repositoryType in repositories)
            services.AddTransient(repositoryType);

        return services;
    }
}