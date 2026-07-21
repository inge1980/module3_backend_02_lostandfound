// To use:
// http://localhost:5179/swagger/index.html
// http://localhost:5179/api/items/
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using webapi.Services;
using webapi.Repositories;
using webapi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ItemDbContext>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default"))
);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Controllers
builder.Services.AddControllers();
//builder.Services.AddSingleton<IItemRepository, InMemoryItemRepository>(); // Used InMemory repository for testing before Postgres was ready
builder.Services.AddScoped<IItemRepository, PostgresItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi(); // http://localhost:5179/openapi/v1.json
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html"); // evt. 404.html

// Ensure database is created
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<ItemDbContext>();
    db.Database.EnsureCreated();
}

app.Run();