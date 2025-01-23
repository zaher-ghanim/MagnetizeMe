using ItemsAPI.Interface;
using ItemsAPI.Services;
using Microsoft.EntityFrameworkCore;
using RepositoryAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IItemsServices, ItemsServices>();
// Register DbContext with the connection string from appsettings.json
//builder.Services.AddDbContext<EcommerceDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//var app = builder.Build();
builder.Services.AddDbContext<EcommerceDbContext>(options => options.UseSqlServer("MagnetizeMe"));
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
