using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace claim_based_auth_sample.API;

public static class APILayerDI
{
    public static IServiceCollection AddAPILayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationConfiguration(configuration);
        services.AddAuthorizationConfiguration(configuration);
        return services;
    }

    public static void AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["TemporalSecretKey"])
                ),
                ClockSkew = TimeSpan.Zero,
            });
    }

    public static void AddAuthorizationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

    }
}
