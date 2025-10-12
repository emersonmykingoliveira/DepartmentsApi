using Departments.BusinessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Services
{
    public interface IDepartmentParser
    {
        Task<List<Department>> ReadFileAsDepartmentsAsync(string filePath);
    }
}
