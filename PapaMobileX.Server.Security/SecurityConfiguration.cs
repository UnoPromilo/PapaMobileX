using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PapaMobileX.Server.Security.Services.Concrete;
using PapaMobileX.Server.Security.Services.Interfaces;

namespace PapaMobileX.Server.Security;

public static class SecurityConfiguration
{
    public static IServiceCollection ConfigureSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        byte[] jwtSecretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>(Constants.JWTSecretConfigKey));
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtSecretKey),
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddTransient<IHashService, HashService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<IAddAccountService, AddAccountService>();
        
        return services;
    }
}