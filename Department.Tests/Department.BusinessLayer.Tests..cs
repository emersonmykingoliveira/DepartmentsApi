using Departments.BusinessLayer.Models;
using Departments.BusinessLayer.Services;
using NSubstitute;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Xunit;

namespace Departments.Tests.BusinessLayer.Services
{
    public class DepartmentParserTests
    {
        private readonly IFileSystem _fileSystem = Substitute.For<IFileSystem>();
        private readonly IFile _file = Substitute.For<IFile>();
        private readonly DepartmentParser _parser;

        public DepartmentParserTests()
        {
            _fileSystem.File.Returns(_file);
            _parser = new DepartmentParser(_fileSystem);
        }

        [Fact]
        public async Task ReadFileAsDepartmentsAsync_ShouldParseValidCsv()
        {
            // Arrange
            var filePath = "departments.csv";
            var csvLines = new[]
            {
                "OID,Title,Color,DepartmentParent_OID",
                "1,US News,#F52612,",
                "2,Crime + Justice,#F52612,1",
                "3,Energy + Environment,#F52612,1",
                "4,Extreme Weather,#F52612,1",
                "5,Space + Science,#F52612,1",
                "6,International News,#EB5F25,",
                "7,Africa,#EB5F25,6",
                "8,Americas,#EB5F25,6",
                "9,Asia,#EB5F25,6",
                "10,Europe,#EB5F25,6"
            };

            _file.ReadAllLinesAsync(filePath).Returns(Task.FromResult(csvLines));

            // Act
            var result = await _parser.ReadFileAsDepartmentsAsync(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);

            var first = result[0];
            Assert.Equal(1, first.Oid);
            Assert.Equal("US News", first.Title);
            Assert.Equal("#F52612", first.Color);
            Assert.Null(first.DepartmentParentOID);

            var last = result[9];
            Assert.Equal(10, last.Oid);
            Assert.Equal(6, last.DepartmentParentOID);
        }

        [Fact]
        public async Task ReadFileAsDepartmentsAsync_ShouldThrow_WhenInvalidOid()
        {
            // Arrange
            var filePath = "invalid.csv";
            var csvLines = new[]
            {
                "OID,Title,Color,DepartmentParent_OID",
                ",Invalid,#123456,"
            };

            _file.ReadAllLinesAsync(filePath).Returns(Task.FromResult(csvLines));

            // Act & Assert
            await Assert.ThrowsAsync<System.ArgumentException>(
                async () => await _parser.ReadFileAsDepartmentsAsync(filePath));
        }

        [Fact]
        public async Task ReadFileAsDepartmentsAsync_ShouldSkipEmptyLines()
        {
            // Arrange
            var filePath = "skip-empty.csv";
            var csvLines = new[]
            {
                "OID,Title,Color,DepartmentParent_OID",
                "1,US News,#F52612,",
                "",
                "2,Crime + Justice,#F52612,1"
            };

            _file.ReadAllLinesAsync(filePath).Returns(Task.FromResult(csvLines));

            // Act
            var result = await _parser.ReadFileAsDepartmentsAsync(filePath);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, d => d.Title == "");
        }
    }
}
