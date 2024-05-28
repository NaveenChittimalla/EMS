using Azure.Core;
using EMS.WebApi.EfCore.Data;
using EMS.WebApi.EfCore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    //options.Filters.Add<EMS.WebApi.EfCore.Controllers.ModelValidationActionFilter>(int.MinValue);
});

builder.Services.AddDbContext<EmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmsEfConnectionString")));

var app = builder.Build();

app.MapGet("/", () => "EMS Web Api!");

app.MapControllers();

app.Run();
