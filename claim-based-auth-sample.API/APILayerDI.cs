using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
    }

    public static void AddAuthorizationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {

    }
}
