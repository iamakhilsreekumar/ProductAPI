using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Behaviour;
using ProductAPI.Errors;
using ProductAPI.Persistence;
using ProductAPI.Repositories;
using System.Net.NetworkInformation;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Management API", Version = "v1" });
});

builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MainConnectionString")));

builder.Services.AddScoped<IMainDbContext, MainDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(x =>x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddSingleton<ProblemDetailsFactory, ProductAPIProblemDetailsFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( x=> x.SwaggerEndpoint($"/swagger/v1/swagger.json", "ProductAPI.API V1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
