using Departments.BusinessLayer.Models;
using System.Collections.Generic;

namespace Departments.BusinessLayer.Services
{
    public interface IDepartmentHierarchyBuilder
    {
        List<DepartmentWithHierarchy> BuildDepartmentHierarchy(List<DepartmentFromFile> departments);
    }
}
