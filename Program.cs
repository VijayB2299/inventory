using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Host configuration
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection string
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// JWT configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
if (jwtSection == null)
{
    throw new Exception("JWT configuration section is missing in appsettings.json");
}

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var jwt = jwtSection.Get<JwtOptions>()!;

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwt.Key))
        };
    });

// Service registration
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddTransient<CorrelationIdMiddleware>();

// Middleware configuration
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("CorrelationScope");

    using (logger.BeginScope(new Dictionary<string, object>
    {
        ["CorrelationId"] = context.TraceIdentifier
    }))
    {
        await next();
    }
});

// Seed admin user and roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<InventoryDbContext>();

    await Seeder.Seed(dbContext);
}

app.Run();
