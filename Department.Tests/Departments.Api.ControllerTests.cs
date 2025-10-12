using Departments.Api.Controllers;
using Departments.BusinessLayer.Models;
using Departments.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Departments.Tests
{
    public class DepartmentsApiControllerTests
    {
        private static (DepartmentsController controller, IDepartmentFileReaderService service, IConfiguration config)
            CreateControllerWithMocks(List<Department> departments)
        {
            var mockService = Substitute.For<IDepartmentFileReaderService>();
            var mockConfig = Substitute.For<IConfiguration>();

            mockConfig["DepartmentFiles:Path"].Returns("fake/path");
            mockService.ReadAllFilesAsync("fake/path").Returns(departments);

            var controller = new DepartmentsController(mockService, mockConfig);
            return (controller, mockService, mockConfig);
        }

        [Fact]
        public async Task GetHierarchy_ReturnsOkWithDepartments_Test()
        {
            // Arrange
            var expected = new List<Department>
            {
                new() { Oid = 1, Title = "HR", Color = "#000000", DepartmentParentOID = 0 }
            };

            var (controller, service, _) = CreateControllerWithMocks(expected);

            // Act
            var result = await controller.GetHierarchy();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<List<Department>>(ok.Value);
            Assert.Single(value);
            Assert.Equal("HR", value[0].Title);

            await service.Received(1).ReadAllFilesAsync("fake/path");
        }

        [Fact]
        public async Task GetHierarchy_ReturnsEmptyList_WhenNoDepartments_Test()
        {
            // Arrange
            var (controller, service, _) = CreateControllerWithMocks(new List<Department>());

            // Act
            var result = await controller.GetHierarchy();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Empty(Assert.IsType<List<Department>>(ok.Value));

            await service.Received(1).ReadAllFilesAsync("fake/path");
        }

        [Fact]
        public async Task GetHierarchy_ReturnsHierarchy_WithChildDepartments_Test()
        {
            // Arrange
            var expected = new List<Department>
            {
                new()
                {
                    Oid = 1, Title = "HR", Color = "#000000",
                    Departments = new List<Department>
                    {
                        new() { Oid = 2, Title = "Recruitment", Color = "#F52612", DepartmentParentOID = 1 },
                        new() { Oid = 3, Title = "Training", Color = "#12AB45", DepartmentParentOID = 1 }
                    }
                }
            };

            var (controller, service, _) = CreateControllerWithMocks(expected);

            // Act
            var result = await controller.GetHierarchy();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<List<Department>>(ok.Value);

            var hr = Assert.Single(value);
            Assert.Equal("HR", hr.Title);
            Assert.Equal("#000000", hr.Color);
            Assert.Equal(2, hr.Departments.Count);
            Assert.Contains(hr.Departments, d => d.Title == "Recruitment" && d.Color == "#F52612");
            Assert.Contains(hr.Departments, d => d.Title == "Training" && d.Color == "#12AB45");

            await service.Received(1).ReadAllFilesAsync("fake/path");
        }

        [Fact]
        public async Task GetHierarchy_ReturnsHierarchy_WithThreeLevelsOfDepartments()
        {
            // Arrange
            var expected = new List<Department>
            {
                new()
                {
                    Oid = 1,
                    Title = "HR",
                    Color = "#000000",
                    DepartmentParentOID = 0,
                    Departments = new List<Department>
                    {
                        new()
                        {
                            Oid = 2,
                            Title = "Recruitment",
                            Color = "#F52612",
                            DepartmentParentOID = 1,
                            Departments = new List<Department>
                            {
                                new()
                                {
                                    Oid = 4,
                                    Title = "Campus Hiring",
                                    Color = "#FFD700",
                                    DepartmentParentOID = 2
                                }
                            }
                        },
                        new()
                        {
                            Oid = 3,
                            Title = "Training",
                            Color = "#12AB45",
                            DepartmentParentOID = 1
                        }
                    }
                }
            };

            var (controller, service, _) = CreateControllerWithMocks(expected);

            // Act
            var result = await controller.GetHierarchy();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var departments = Assert.IsType<List<Department>>(ok.Value);

            var hr = Assert.Single(departments);
            Assert.Equal("HR", hr.Title);
            Assert.Equal(2, hr.Departments.Count);

            var recruitment = hr.Departments.First(d => d.Title == "Recruitment");
            Assert.Single(recruitment.Departments);
            var campusHiring = recruitment.Departments.First();
            Assert.Equal("Campus Hiring", campusHiring.Title);
            Assert.Equal("#FFD700", campusHiring.Color);

            var training = hr.Departments.First(d => d.Title == "Training");
            Assert.Equal("#12AB45", training.Color);

            await service.Received(1).ReadAllFilesAsync("fake/path");
        }
    }
}
