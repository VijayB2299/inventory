using Inventory.Services;

// Host configuration
var builder = WebApplication.CreateBuilder(args);

// Service registration
builder.Services.AddControllers();
builder.Services.AddSingleton<IInventoryService, InventoryService>();

// Middleware configuration
var app = builder.Build();

app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();
app.Run();