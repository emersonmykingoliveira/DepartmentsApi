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

        public async Task<List<Department>> ReadAllFilesAsync(string filePath)
        {
            List<Department> departmentsCollection = new List<Department>();

            string[] files = Directory.GetFiles(filePath);

            foreach (string file in files)
            {
                var departments = await ReadFileAsDepartmentsAsync(file);
                if (departments is not null)
                    departmentsCollection.AddRange(departments);
            }
          
            return SortDepartmentsHierarcy(departmentsCollection);

        }

        private async Task<List<Department>> ReadFileAsDepartmentsAsync(string file)
        {
            List<Department> departments = new List<Department>();

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {
                    await reader.ReadLineAsync();//To skip the header line

                    while (await reader.ReadLineAsync() is string line)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var department = TryParseLineAsDepartment(line);

                        if (department is not null)
                            departments.Add(department);
                    }
                }
            }

            return departments;
        }

        private List<Department> SortDepartmentsHierarcy(List<Department> departmentsCollection)
        {
            var departmentsDictionary = CreateDepartmentDictionary(departmentsCollection);

            var departmentsRoots = new List<Department>();

            foreach (var department in departmentsCollection)
            {
                if (departmentsDictionary.TryGetValue(department.DepartmentParentOID, out var parent))
                {
                    parent.Departments.Add(department);
                }
                else
                {
                    departmentsRoots.Add(department);
                }
            }

            return departmentsRoots;
        }

        private Dictionary<int, Department> CreateDepartmentDictionary(List<Department> departmentsCollection)
        {
            Dictionary<int, Department> departmentDictionary = new Dictionary<int, Department>();

            foreach (var department in departmentsCollection)
            {
                departmentDictionary.TryAdd(department.Oid, department);
            }

            return departmentDictionary;
        }

        private Department TryParseLineAsDepartment(string line)
        {
            var content = line.Split(',');

            Department departmentWithParent = new Department();

            if (int.TryParse(content[0], out int oID))
                departmentWithParent.Oid = oID;

            departmentWithParent.Title = content[1];

            departmentWithParent.Color = content[2];

            if (int.TryParse(content[3], out int departmentParentOID))
                departmentWithParent.DepartmentParentOID = departmentParentOID;

            return departmentWithParent;
        }
    }
}
