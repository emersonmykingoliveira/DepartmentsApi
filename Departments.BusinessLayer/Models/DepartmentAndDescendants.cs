using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Models
{
    public class DepartmentAndDescendants
    {
        public int Oid { get; set; }
        public string? Title { get; set; }
        public int NumDescendants { get; set; }
        public string? Color { get; set; }
        public List<DepartmentAndDescendants>? Departments { get; set; }
    }
}