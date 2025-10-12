using Departments.Api.EndPoints;
using Departments.Api.Middleware;
using Departments.BusinessLayer.Models;
using Departments.BusinessLayer.Services;
using Microsoft.OpenApi.Models;

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

//Dependency registrations
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

//Endpoints
app.GetDepartmentsHierarcy();

app.Run();