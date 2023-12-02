using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace claim_based_auth_sample.Application;

public static class ApplicationLayerDI
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterAutoMapper(configuration);
        services.AddServices(configuration);
        return services;
    }

    public static void RegisterAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(AutoMapperMarker));
    }
    
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<INotesService, NotesService>();
    }
}
