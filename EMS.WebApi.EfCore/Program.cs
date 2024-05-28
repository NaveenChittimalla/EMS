using EMS.WebApi.EfCore.Data;
using EMS.WebApi.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    //options.Filters.Add<EMS.WebApi.EfCore.Controllers.ModelValidationActionFilter>(int.MinValue);
});

builder.Services.AddDbContext<EmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmsEfConnectionString")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeV3Repository, EmployeeV3Repository>();

var app = builder.Build();

app.MapGet("/", () => "EMS Web Api!");

app.MapControllers();

app.Run();
