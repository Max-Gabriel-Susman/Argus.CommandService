using Microsoft.EntityFrameworkCore; 
using Argus.InventoryService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<InventoryContext>(opt =>
    opt.UseSqlServer(connectionString));

var app = builder.Build();

// Right now the application is always in dev mode but if
// you were to productionalize it this would have to change.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();


app.MapControllers().WithOpenApi();
app.Run();

