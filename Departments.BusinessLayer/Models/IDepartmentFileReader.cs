using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Models
{
    public interface IDepartmentFileReader
    {
        string FilePath { get; set; }
        Task<List<DepartmentAndDescendants>> ReadAllFiles();
    }
}
