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
        public string FilePath { get; set; }

        public DepartmentFileReaderService(string filePath)
        {
            FilePath = filePath;
        }

        public async Task<List<DepartmentResult>> ReadAllFilesAsync()
        {
            DepartmentCollection departmentCollection = new DepartmentCollection();

            string[] files = Directory.GetFiles(FilePath);

            foreach (string file in files)
            {
                var departments = await ReadFileAsDepartmentsAsync(file);
                if (departments is not null)
                    departmentCollection.Departments.AddRange(departments);
            }
          
            return SortDepartmentsHierarcy(departmentCollection);

        }

        private async Task<List<Department>> ReadFileAsDepartmentsAsync(string file)
        {
            List<Department> departments = new List<Department>();

            using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
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

        private List<DepartmentResult> SortDepartmentsHierarcy(DepartmentCollection departmentCollection)
        {
            var departmentsDictionary = departments.ToDictionary(d => d.Oid);

            var departmentsRoots = new List<Department>();

            foreach (var department in departments)
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
