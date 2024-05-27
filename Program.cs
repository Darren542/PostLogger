using Microsoft.EntityFrameworkCore;
using SaveLogger.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
// ?? throw new InvalidOperationException("Connection string not found.");

var connectionString = "DataSource=SaveLog.db;Cache=Shared";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));


builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello, world!");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log or handle the exception as needed
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    }
}

app.Run();
