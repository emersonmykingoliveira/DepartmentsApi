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
        [Fact]
        public async Task GetHierarchy_ReturnsOkWithDepartments_Test()
        {
            // Arrange
            var mockService = Substitute.For<IDepartmentFileReaderService>();
            var mockConfig = Substitute.For<IConfiguration>();

            var expectedDepartments = new List<Department>
            {
                new Department { Oid = 1, Title = "HR", Color = "#000000", DepartmentParentOID = 0 }
            };

            mockConfig["DepartmentFiles:Path"].Returns("fake/path");
            mockService.ReadAllFilesAsync("fake/path").Returns(expectedDepartments);

            var controller = new DepartmentsController(mockService, mockConfig);

            // Act
            var result = await controller.GetHierarchy();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<List<Department>>(okResult.Value);
            Assert.Single(value);
            Assert.Equal("HR", value[0].Title);

            await mockService.Received(1).ReadAllFilesAsync("fake/path");

        }

        [Fact]
        public async Task GetHierarchy_ReturnsEmptyList_WhenNoDepartments_Test()
        {
            // Arrange
            var mockService = Substitute.For<IDepartmentFileReaderService>();
            var mockConfig = Substitute.For<IConfiguration>();

            mockConfig["DepartmentFiles:Path"].Returns("fake/path");
            mockService.ReadAllFilesAsync("fake/path").Returns(new List<Department>());

            var controller = new DepartmentsController(mockService, mockConfig);

            // Act
            var result = await controller.GetHierarchy();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<List<Department>>(okResult.Value);
            Assert.Empty(value);

            await mockService.Received(1).ReadAllFilesAsync("fake/path");
        }

        [Fact]
        public async Task GetHierarchy_ReturnsHierarchy_WhenDepartmentsHaveChildren_Test()
        {
            // Arrange
            var mockService = Substitute.For<IDepartmentFileReaderService>();
            var mockConfig = Substitute.For<IConfiguration>();

            var expectedDepartments = new List<Department>
            {
                new Department
                {
                    Oid = 1,
                    Title = "HR",
                    Color = "#000000",
                    DepartmentParentOID = 0,
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Oid = 2,
                            Title = "Recruitment",
                            Color = "#F52612",
                            DepartmentParentOID = 1
                        },
                        new Department
                        {
                            Oid = 3,
                            Title = "Training",
                            Color = "#12AB45",
                            DepartmentParentOID = 1
                        }
                    }
                }
            };

            mockConfig["DepartmentFiles:Path"].Returns("fake/path");
            mockService.ReadAllFilesAsync("fake/path").Returns(expectedDepartments);

            var controller = new DepartmentsController(mockService, mockConfig);

            // Act
            var result = await controller.GetHierarchy();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<List<Department>>(okResult.Value);

            Assert.Single(value);
            var hr = value.First();

            Assert.Equal("HR", hr.Title);
            Assert.Equal("#000000", hr.Color);
            Assert.Equal(2, hr.Departments.Count);

            var recruitment = hr.Departments.First(d => d.Title == "Recruitment");
            Assert.Equal("#F52612", recruitment.Color);

            var training = hr.Departments.First(d => d.Title == "Training");
            Assert.Equal("#12AB45", training.Color);

            await mockService.Received(1).ReadAllFilesAsync("fake/path");
        }
    }
}
