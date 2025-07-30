using System.Text;
using EmployeeManagement.Api.Middlewares;
using EmployeeManagement.Entities.Data;
using EmployeeManagement.Entities.Models;
using EmployeeManagement.Entities.Shared.Convertor;
using EmployeeManagement.Repositories.Implementation;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Implementation;
using EmployeeManagement.Services.Implementations;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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

var configuration = builder.Configuration;

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
        ValidIssuer = configuration["JwtSettings:Issuer"],
        ValidAudience = configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
        ClockSkew = TimeSpan.Zero // Remove default 5-minute clock skew
    };
    // options.Events = new JwtBearerEvents
    // {
    //     OnMessageReceived = context =>
    //     {
    //         if (context.Request.Cookies.ContainsKey("RefreshToken"))
    //         {
    //             context.Token = context.Request.Cookies["RefreshToken"];
    //         }
    //         return Task.CompletedTask;
    //     }
    // };

});
builder.Services.AddAuthorization();

// for returning validation error in ApiResponse Formate
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    var isDev = builder.Environment.IsDevelopment();

    options.InvalidModelStateResponseFactory = context =>
        ValidationConvertor.CreateValidationErrorResponse(context, isDev);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var AllowSpecificOrigins = "_allowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:4200") // <--- Allow your Angular app's origin
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

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// Set NLog as the logging provider for the application.
// Configures the host to use NLog
builder.Host.UseNLog();

var app = builder.Build();

// IMPORTANT: Add the global exception middleware early in the pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

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
