//using Departments.BusinessLayer.Models;
//using Departments.BusinessLayer.Services;
//using NSubstitute;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Departments.Tests.Services
//{
//    public class DepartmentFileReaderServiceTests
//    {
//        [Fact]
//        public async Task ReadAllFilesAsync_EmptyDirectory_ReturnsEmptyList()
//        {
//            // Arrange
//            var directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
//            Directory.CreateDirectory(directoryPath);

//            var service = new DepartmentFileReaderService();

//            // Act
//            var result = await service.ReadAllFilesAsync(directoryPath);

//            // Assert
//            Assert.Empty(result);
//        }

//        [Fact]
//        public void TryParseDepartment_ValidLine_ReturnsDepartment()
//        {
//            // Arrange
//            var line = "1,Finance,#F52612,0";
//            var method = typeof(DepartmentFileReaderService)
//                .GetMethod("TryParseDepartment", BindingFlags.NonPublic | BindingFlags.Instance);
//            var service = new DepartmentFileReaderService();

//            // Act
//            var result = (Department?)method?.Invoke(service, new object[] { line, 1 });

//            // Assert
//            Assert.Equal(1, result?.Oid);
//            Assert.Equal("Finance", result?.Title);
//            Assert.Equal("#F52612", result?.Color);
//            Assert.Equal(0, result?.DepartmentParentOID);
//        }

//        [Fact]
//        public void BuildDepartmentHierarchy_ShouldLinkChildDepartments()
//        {
//            // Arrange
//            var parent = new Department { Oid = 1, Title = "Parent", DepartmentParentOID = 0, Departments = new List<Department>() };
//            var child = new Department { Oid = 2, Title = "Child", DepartmentParentOID = 1, Departments = new List<Department>() };

//            var departments = new List<Department> { parent, child };

//            var method = typeof(DepartmentFileReaderService)
//                .GetMethod("BuildDepartmentHierarchy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

//            var service = new DepartmentFileReaderService();

//            // Act
//            var result = (List<Department>?)method?.Invoke(service, new object[] { departments });

//            // Assert
//            Assert.Single(result!); // Root only
//            Assert.Single(result![0].Departments);
//            Assert.Equal("Child", result[0].Departments[0].Title);
//        }
//    }
//}
