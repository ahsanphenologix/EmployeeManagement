using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.AppDbContext;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Repository;
using EmployeeManagement.API.Models.DatabaseModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("https://localhost:7013/", "https://localhost:7013/")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var serverVersion = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"));

builder.Services.AddDbContext<CustomerDbContext>(item =>
        item.UseMySql(builder.Configuration.GetConnectionString("Default"), serverVersion));

builder.Services.AddScoped<IRepository<CustomerDbModel>, CustomerRepository>();
builder.Services.AddScoped<IRepository<CustomerImageDbModel>, CustomerImageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
