using Departments.BusinessLayer.Models;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;

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

        public async Task<List<DepartmentWithHierarchy>> ReadAllFilesAsync(string directoryPath)
        {
            var allDepartments = new List<DepartmentFromFile>();

            foreach (var file in _fileSystem.Directory.EnumerateFiles(directoryPath))
            {
                var departments = await _parser.ReadFileAsDepartmentsAsync(file);
                if (departments?.Count > 0)
                    allDepartments.AddRange(departments);
            }

            return _hierarchyBuilder.BuildDepartmentHierarchy(allDepartments);
        }
    }
}
