using CalendarManagement.API.Endpoints;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Application.Services;
using CalendarManagement.Infrastructure.Data;
using CalendarManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<CalendarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Customers
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Activities
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

// Locations
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    // Bara i produktion – under utveckling stör den React-anropen
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");

app.MapControllers();


app.MapActivityEndpoints();
app.MapCustomerEndpoints();
app.MapLocationEndpoints();

app.Run();