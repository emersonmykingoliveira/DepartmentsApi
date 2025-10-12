using Departments.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentParser : IDepartmentParser
    {
        private readonly IFileSystem _fileSystem;

        public DepartmentParser(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<List<DepartmentFromFile>> ReadFileAsDepartmentsAsync(string filePath)
        {
            var departments = new List<DepartmentFromFile>();

            string[] lines = await _fileSystem.File.ReadAllLinesAsync(filePath);

            int lineNumber = 0;

            foreach (var line in lines.Skip(1))
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (TryParseDepartment(line, lineNumber) is { } department)
                    departments.Add(department);
            }

            return departments;
        }

        private DepartmentFromFile TryParseDepartment(string line, int lineNumber)
        {
            var content = line.Split(',');

            DepartmentFromFile departmentWithParent = new DepartmentFromFile();

            //OID
            if (int.TryParse(content[0], out int oID))
                departmentWithParent.Oid = oID;
            else
                throw new ArgumentException($"Line number {lineNumber}, error parsing OID");

            //Title
            if (string.IsNullOrWhiteSpace(content[1]))
                throw new ArgumentException($"Line number {lineNumber}, error parsing Title");

            departmentWithParent.Title = content[1];

            //Color
            if (string.IsNullOrWhiteSpace(content[2]))
                throw new ArgumentException($"Line number {lineNumber}, error parsing Color");

            departmentWithParent.Color = content[2];

            //DepartmentParentOID
            if (!string.IsNullOrWhiteSpace(content[3]))
            {
                if (int.TryParse(content[3], out int departmentParentOID))
                    departmentWithParent.DepartmentParentOID = departmentParentOID;
                else
                    throw new ArgumentException($"Line number {lineNumber}, error parsing DepartmentParentOID");
            }

            return departmentWithParent;
        }
    }
}
