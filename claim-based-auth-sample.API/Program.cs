using claim_based_auth_sample.DataAccess;
using claim_based_auth_sample.Application;
using claim_based_auth_sample.API;

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

app.Run();

