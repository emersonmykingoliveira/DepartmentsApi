using Departments.BusinessLayer.Models;
using System.Collections.Generic;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentHierarchyBuilder : IDepartmentHierarchyBuilder
    {
        public List<DepartmentWithHierarchy> BuildDepartmentHierarchy(List<DepartmentFromFile> departments)
        {
            var dict = BuildDepartmentsDictionary(departments);
            List<DepartmentWithHierarchy> root = new List<DepartmentWithHierarchy>();

            foreach (var dept in dict.Values)
            {
                if (dict.TryGetValue(dept.DepartmentParentOID ?? 0, out var parent))
                    parent.Departments.Add(dept);
                else
                    root.Add(dept);
            }

            return root;
        }

        private Dictionary<int, DepartmentWithHierarchy> BuildDepartmentsDictionary(List<DepartmentFromFile> departmentsCollection)
        {
            Dictionary<int, DepartmentWithHierarchy> departmentDictionary = new Dictionary<int, DepartmentWithHierarchy>();

            foreach (var department in departmentsCollection)
            {
                departmentDictionary.TryAdd(department.Oid, new DepartmentWithHierarchy
                {
                    Color = department.Color,
                    Oid = department.Oid,
                    Title = department.Title,
                    DepartmentParentOID = department.DepartmentParentOID
                });
            }

            return departmentDictionary;
        }
    }
}
