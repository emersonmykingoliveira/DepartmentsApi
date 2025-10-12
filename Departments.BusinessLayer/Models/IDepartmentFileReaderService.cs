using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Models
{
    public interface IDepartmentFileReaderService
    {
        string FilePath { get; set; }
        Task<List<Department>> ReadAllFilesAsync();
    }
}
