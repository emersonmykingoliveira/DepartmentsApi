using Departments.BusinessLayer.Models;
using Departments.BusinessLayer.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Departments.Tests.Services
{
    public class DepartmentFileReaderServiceTests
    {
        private readonly DepartmentFileReaderService _service;

        public DepartmentFileReaderServiceTests()
        {
            _service = new DepartmentFileReaderService();
        }

        [Fact]
        public void TryParseDepartment_ValidLine_ReturnsDepartment()
        {
            // Arrange
            var line = "1,Finance,Blue,0";
            var method = typeof(DepartmentFileReaderService)
                .GetMethod("TryParseDepartment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (Department)method.Invoke(_service, new object[] { line, 1 });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Oid);
            Assert.Equal("Finance", result.Title);
            Assert.Equal("Blue", result.Color);
            Assert.Equal(0, result.DepartmentParentOID);
        }

        [Fact]
        public void TryParseDepartment_InvalidOid_ThrowsArgumentException()
        {
            // Arrange
            var line = "abc,Finance,Blue,0";
            var method = typeof(DepartmentFileReaderService)
                .GetMethod("TryParseDepartment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act & Assert
            Assert.Throws<TargetInvocationException>(() =>
                method.Invoke(_service, new object[] { line, 1 }));
        }

        [Fact]
        public void BuildDepartmentHierarchy_ShouldNestDepartmentsCorrectly()
        {
            // Arrange
            var parent = new Department { Oid = 1, Title = "Parent", DepartmentParentOID = 0, Departments = new List<Department>() };
            var child = new Department { Oid = 2, Title = "Child", DepartmentParentOID = 1, Departments = new List<Department>() };

            var method = typeof(DepartmentFileReaderService)
                .GetMethod("BuildDepartmentHierarchy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = (List<Department>)method.Invoke(_service, new object[] { new List<Department> { parent, child } });

            // Assert
            Assert.Single(result); // Only root department
            Assert.Equal("Parent", result[0].Title);
            Assert.Single(result[0].Departments);
            Assert.Equal("Child", result[0].Departments[0].Title);
        }

        [Fact]
        public async Task ReadAllFilesAsync_EmptyDirectory_ReturnsEmptyList()
        {
            // Arrange
            var directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(directoryPath);

            // Act
            var result = await _service.ReadAllFilesAsync(directoryPath);

            // Assert
            Assert.Empty(result);
        }
    }
}
