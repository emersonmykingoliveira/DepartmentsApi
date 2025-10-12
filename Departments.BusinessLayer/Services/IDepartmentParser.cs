using Departments.BusinessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Services
{
    public interface IDepartmentParser
    {
        Task<List<DepartmentFromFile>> ReadFileAsDepartmentsAsync(string filePath);
    }
}
