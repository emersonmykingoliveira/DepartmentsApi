using Departments.BusinessLayer.Models;
using System.Collections.Generic;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentHierarchyBuilder : IDepartmentHierarchyBuilder
    {
        public List<Department> BuildDepartmentHierarchy(List<Department> departments)
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
    }
}
