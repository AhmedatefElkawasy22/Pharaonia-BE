var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddControllers();

builder.Services.AddDependencyInjectionServices();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddIdentityConfiguration();
builder.Services.AddCorsPolicy();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddAuthenticationConfiguration(builder.Configuration);

// Enable API Explorer for Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.ConfigureMiddlewarePipeline();
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DataSeeder.SeedAdminUser(serviceProvider);
}
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DataSeeder.SeedAdminUser(serviceProvider);
}


app.Run();
