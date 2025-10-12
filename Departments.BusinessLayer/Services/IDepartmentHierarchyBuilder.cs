using Departments.BusinessLayer.Models;
using System.Collections.Generic;

namespace Departments.BusinessLayer.Services
{
    public interface IDepartmentHierarchyBuilder
    {
        List<Department> BuildDepartmentHierarchy(List<Department> departments);
    }
}
