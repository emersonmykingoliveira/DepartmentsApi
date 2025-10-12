using Departments.BusinessLayer.Models;
using System.IO.Abstractions;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentFileReaderService : IDepartmentFileReaderService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IDepartmentParser _parser;
        private readonly IDepartmentHierarchyBuilder _hierarchyBuilder;

        public DepartmentFileReaderService(
            IFileSystem fileSystem,
            IDepartmentParser parser,
            IDepartmentHierarchyBuilder hierarchyBuilder)
        {
            _fileSystem = fileSystem;
            _parser = parser;
            _hierarchyBuilder = hierarchyBuilder;
        }

        public async Task<List<Department>> ReadAllFilesAsync(string directoryPath)
        {
            var allDepartments = new List<Department>();

            foreach (var file in _fileSystem.Directory.EnumerateFiles(directoryPath))
            {
                var departments = await _parser.ParseFileAsync(file);
                allDepartments.AddRange(departments);
            }

            return _hierarchyBuilder.BuildHierarchy(allDepartments);
        }
    }
}
