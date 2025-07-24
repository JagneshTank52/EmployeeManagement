using System.Text;
using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Repositories.Implementation;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Implementation;
using EmployeeManagement.Services.Implementations;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EmpManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EmpDatabase")));

builder.Services.AddControllers()
 .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Authentication
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
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero // Remove default 5-minute clock skew
    };
});
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var AllowSpecificOrigins = "_allowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200") // <--- Allow your Angular app's origin
                                 .AllowAnyHeader() // Allow all headers
                                 .AllowAnyMethod() // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
                                 .AllowCredentials(); // <--- Crucial if you are sending cookies or authentication headers
                      });
});

// Clears built-in providers
builder.Logging.ClearProviders();

builder.Services.AddAutoMapper(typeof(MappingPeofile));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<JwtSettings>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// Set NLog as the logging provider for the application.
// Configures the host to use NLog
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(AllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
