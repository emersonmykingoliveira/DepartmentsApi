using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Models
{
    internal class Department
    {
        [JsonIgnore]
        public int DepartmentParentOID { get; set; }
        public int Oid { get; set; }
        public string? Title { get; set; }
        public int NumDescendants => Departments.Count();
        public string? Color { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();
        
    }
}