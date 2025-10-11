using Departments.Api.EndPoints;
using Departments.BusinessLayer.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

builder.Services.AddScoped<IDepartmentFileReader, DepartmentFileReader>();

app.UseHttpsRedirection();

//Endpoints
app.GetDepartmentsHierarcy();

app.Run();