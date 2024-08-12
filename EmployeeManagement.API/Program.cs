using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.AppDbContext;
using EmployeeManagement.API.Interfaces;
using EmployeeManagement.API.Repository;
using EmployeeManagement.API.Models.DatabaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EmployeeManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);


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


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<CustomerDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});



//builder.Services.AddAuthorization(options =>
//{
    

//    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ensure roles are created
//using (var scope = app.Services.CreateScope())
//{
//    var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
//    var roles = await roleService.GetAllRolesAsync();

//    // Configure authorization policies dynamically
//    builder.Services.AddAuthorization(options =>
//    {
//        foreach (var role in roles)
//        {
//            options.AddPolicy($"{role}Policy", policy => policy.RequireRole(role));
//        }
//    });
//}

builder.Services.AddScoped<IRepository<CustomerDbModel>, CustomerRepository>();
builder.Services.AddScoped<IRepository<CustomerImageDbModel>, CustomerImageRepository>();
builder.Services.AddTransient<RoleService>();

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
