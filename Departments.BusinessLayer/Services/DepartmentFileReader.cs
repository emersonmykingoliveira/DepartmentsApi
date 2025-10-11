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

        public async Task<List<Department>> ReadAllFiles()
        {
            List<Department> departments = new List<Department>();
            List<DepartmentWithParent> departmentWithParents = new List<DepartmentWithParent>();
            string[] files = Directory.GetFiles(FilePath);

            foreach (string file in files)
            {
                using var stream = File.OpenRead(file);
                using var reader = new StreamReader(stream);

                string? line;
                while ((line = await reader.ReadLineAsync()) is not null)
                {
                    var result = TryParseFileContent(line);
                    departmentWithParents.Add(result);
                }
            }

            return SortDepartmentsHierarcy(departmentWithParents);
        }

        private List<Department> SortDepartmentsHierarcy(List<DepartmentWithParent> departmentWithParents)
        {
            throw new NotImplementedException();
        }

        private DepartmentWithParent TryParseFileContent(string line)
        {
            var content = line.Split(',');

            DepartmentWithParent departmentWithParent = new DepartmentWithParent();

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
