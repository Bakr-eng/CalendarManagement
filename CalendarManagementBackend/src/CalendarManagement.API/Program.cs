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

// Users
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Activities
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

// Locations
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
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

app.MapGet("/api/exempel", async (CalendarDbContext db) =>
    await db.Exempel.ToListAsync());

app.MapActivityEndpoints();
app.MapCustomerEndpoints();
app.MapLocationEndpoints();
app.MapUserEndpoints();

app.Run();