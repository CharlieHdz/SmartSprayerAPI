using Microsoft.EntityFrameworkCore;
using SmartSprayerAPI.Data;
using SmartSprayerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
                                            options.UseSqlServer("Server=tcp:smartsprayer-sql-us.database.windows.net,1433;Initial Catalog=SmartSprayerDB;Persist Security Info=False;User ID=sqladmin;Password=SprayerAdmin!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
builder.Services.AddControllers();
builder.Services.AddScoped<SensorService>();
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

app.Run();
