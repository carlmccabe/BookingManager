using BookingManager.API.Data;
using BookingManager.API.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

// Add Db Settings

builder.Services.AddOptions();
builder.Services.Configure<DbSettings>(
    builder.Configuration.GetSection("DbSettings"));


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<IDbContext>();
    
    // Seed data if necessary
    dbContext.SeedDataIfNecessary();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IBookingService, BookingService>();
    services.AddTransient<IRoomService, RoomService>();
    services.AddSingleton<IDbContext, DbContext>();
}