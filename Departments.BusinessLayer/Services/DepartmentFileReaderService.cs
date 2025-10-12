using Departments.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentFileReaderService : IDepartmentFileReaderService
    {
        public async Task<List<Department>> ReadAllFilesAsync(string directoryPath)
        {
            var allDepartments = new List<Department>();

            foreach (var file in Directory.EnumerateFiles(directoryPath))
            {
                var departments = await ReadFileAsDepartmentsAsync(file);
                if (departments?.Count > 0)
                    allDepartments.AddRange(departments);
            }

            return BuildDepartmentHierarchy(allDepartments);

        }

        private async Task<List<Department>> ReadFileAsDepartmentsAsync(string filePath)
        {
            var departments = new List<Department>();

            using var reader = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

            int lineNumber = 0;

            await reader.ReadLineAsync();//Skip header

            lineNumber++;

            while (await reader.ReadLineAsync() is { } line)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (TryParseDepartment(line, lineNumber) is { } department)
                    departments.Add(department);
            }

            return departments;
        }

        private List<Department> BuildDepartmentHierarchy(List<Department> departments)
        {
            var dict = BuildDepartmentsDictionary(departments);
            var roots = new List<Department>();

            foreach (var dept in departments)
            {
                if (dict.TryGetValue(dept.DepartmentParentOID, out var parent))
                    parent.Departments.Add(dept);
                else
                    roots.Add(dept);
            }

            return roots;
        }


        private Dictionary<int, Department> BuildDepartmentsDictionary(List<Department> departmentsCollection)
        {
            Dictionary<int, Department> departmentDictionary = new Dictionary<int, Department>();

            foreach (var department in departmentsCollection)
            {
                departmentDictionary.TryAdd(department.Oid, department);
            }

            return departmentDictionary;
        }

        private Department TryParseDepartment(string line, int lineNumber)
        {
            var content = line.Split(',');

            Department departmentWithParent = new Department();

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

            if (int.TryParse(content[3], out int departmentParentOID))
                departmentWithParent.DepartmentParentOID = departmentParentOID;
            else
                throw new ArgumentException($"Line number {lineNumber}, error parsing DepartmentParentOID");

            return departmentWithParent;
        }
    }
}
