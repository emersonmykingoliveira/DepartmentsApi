using Departments.Api.Middleware;
using Departments.BusinessLayer.Services;
using Microsoft.OpenApi.Models;
using System.IO.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Department Hierarchy API",
        Version = "v1",
        Description = "API for displaying department hierarchy with descendant counts."
    });
});

builder.Services.AddControllers();

//Dependency registrations
builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddScoped<IDepartmentParser, DepartmentParser>();
builder.Services.AddScoped<IDepartmentHierarchyBuilder, DepartmentHierarchyBuilder>();
builder.Services.AddScoped<IDepartmentFileReaderService, DepartmentFileReaderService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Middleware for global error handling when parsing the file
app.UseMiddleware<FileExceptionMiddleware>();

app.MapControllers();

app.Run();