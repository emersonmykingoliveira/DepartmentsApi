using Departments.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentFileReader : IDepartmentFileReader
    {
        public string FilePath { get; set; }

        public DepartmentFileReader(string filePath)
        {
            FilePath = filePath;
        }

        public async Task<List<DepartmentAndDescendants>> ReadAllFiles()
        {
            List<Department> departments = new List<Department>();
            string[] files = Directory.GetFiles(FilePath);

            foreach (string file in files)
            {
                using var stream = File.OpenRead(file);
                using var reader = new StreamReader(stream);

                string? line;
                while ((line = await reader.ReadLineAsync()) is not null)
                {
                    var result = TryParseFileContent(line);
                    departments.Add(result);
                }
            }

            return SortDepartmentsHierarcy(departments);
        }

        private List<DepartmentAndDescendants> SortDepartmentsHierarcy(List<Department> departments)
        {
            var departmentsDictionary = departments.ToDictionary(d => d.Oid);

            foreach (var department in departments)
            {

            }

        }

        private Department TryParseFileContent(string line)
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
