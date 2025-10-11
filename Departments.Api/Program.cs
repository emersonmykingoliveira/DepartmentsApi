using Departments.Api.EndPoints;
using Departments.BusinessLayer.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDepartmentFileReader, DepartmentFileReader>();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//Endpoints
app.GetDepartmentsHierarcy();

app.Run();