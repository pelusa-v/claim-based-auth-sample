using claim_based_auth_sample.DataAccess;
using claim_based_auth_sample.Application;
using claim_based_auth_sample.API;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataAccessLayer(builder.Configuration)
    .AddApplicationLayer(builder.Configuration)
    .AddAPILayer(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyBuilder =>
    corsPolicyBuilder
        .AllowAnyHeader()
        .AllowAnyOrigin()
    );

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Guest", "Admin", "Manager" };

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using(var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string admin_email = "admin@admin.com";
    string admin_password = "Admin123.";   
    
    if(await userManager.FindByEmailAsync(admin_email) == null)
    {
        var user = new IdentityUser
        {
            UserName = admin_email,
            Email = admin_email
        };
        await userManager.CreateAsync(user, admin_password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();

