using EmployeeManagement.Entities.Data;
using EmployeeManagement.Repositories.Implementation;
using EmployeeManagement.Repositories.Interface;
using EmployeeManagement.Services.Helpers;
using EmployeeManagement.Services.Implementation;
using EmployeeManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clears built-in providers
builder.Logging.ClearProviders();

builder.Services.AddAutoMapper(typeof(MappingPeofile));
builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));


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

app.UseAuthorization();

app.MapControllers();

app.Run();
