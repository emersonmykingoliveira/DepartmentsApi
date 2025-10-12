using Departments.Api.Controllers;
using Departments.BusinessLayer.Models;
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
    }
}
