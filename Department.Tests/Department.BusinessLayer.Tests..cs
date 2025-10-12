using Departments.BusinessLayer.Models;
using Departments.BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Departments.Tests
{
    public class DepartmentBusinessLayerTests
    {
        [Fact]
        public void TryParseDepartment_ParsesValidLineCorrectly_Test()
        {
            // Arrange
            var service = new DepartmentFileReaderService();
            var method = typeof(IDepartmentFileReaderService)
                .GetMethod("TryParseDepartment", BindingFlags.NonPublic | BindingFlags.Instance);

            string line = "1,HR,#000000,0";

            // Act
            var department = (Department)method.Invoke(service, new object[] { line, 1 });

            // Assert
            Assert.Equal(1, department.Oid);
            Assert.Equal("HR", department.Title);
            Assert.Equal("#000000", department.Color);
            Assert.Equal(0, department.DepartmentParentOID);
        }

        [Fact]
        public void TryParseDepartment_Throws_WhenTitleMissing()
        {
            // Arrange
            var service = new DepartmentFileReaderService();
            var method = typeof(IDepartmentFileReaderService)
                .GetMethod("TryParseDepartment", BindingFlags.NonPublic | BindingFlags.Instance);

            string invalidLine = "1,,#000000,0";

            // Act & Assert
            var ex = Assert.Throws<TargetInvocationException>(() =>
                method.Invoke(service, new object[] { invalidLine, 2 }));

            Assert.Contains("error parsing Title", ex.InnerException.Message);
        }

        [Fact]
        public void BuildDepartmentHierarchy_BuildsCorrectTree()
        {
            // Arrange
            var service = new DepartmentFileReaderService();
            var method = typeof(IDepartmentFileReaderService)
                .GetMethod("BuildDepartmentHierarchy", BindingFlags.NonPublic | BindingFlags.Instance);

            var departments = new List<Department>
            {
                new Department { Oid = 1, Title = "Root", Color = "#000000", DepartmentParentOID = 0 },
                new Department { Oid = 2, Title = "Child", Color = "#123456", DepartmentParentOID = 1 },
                new Department { Oid = 3, Title = "GrandChild", Color = "#F52612", DepartmentParentOID = 2 }
            };

            // Act
            var result = (List<Department>)method.Invoke(service, new object[] { departments });

            // Assert
            Assert.Single(result); // only one root
            var root = result[0];
            Assert.Equal("Root", root.Title);
            Assert.Single(root.Departments);
            Assert.Equal("Child", root.Departments[0].Title);
            Assert.Single(root.Departments[0].Departments);
            Assert.Equal("GrandChild", root.Departments[0].Departments[0].Title);
        }
    }
}
