using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Models
{
    internal class Department
    {
        public int Oid { get; set; }
        public string? Title { get; set; }
        public string? Color { get; set; }
        public int DepartmentParentOID { get; set; }
    }
}
