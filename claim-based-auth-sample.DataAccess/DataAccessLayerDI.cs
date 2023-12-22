using claim_based_auth_sample.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace claim_based_auth_sample.DataAccess;

public static class DataAccessLayerDI
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);
        services.AddRepositories(configuration);
        services.AddIdentityEntities(configuration);
        return services;
    }

    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseMySQL(configuration.GetConnectionString("Db")));
    }

    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICommonCrudRepository<Note>, NoteRepository>();
    }

    public static void AddIdentityEntities(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}
