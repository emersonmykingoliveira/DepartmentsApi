using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Models
{
    public class Department
    {
        [JsonIgnore]
        public int DepartmentParentOID { get; set; }
        public int Oid { get; set; }
        public string? Title { get; set; }
        public int NumDescendants => CountDescendants();
        public string? Color { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();

        public int CountDescendants()
        {
            if (Departments is null || Departments.Count == 0)
                return 0;

            int total = Departments.Count;
            foreach (var child in Departments)
            {
                total += child.CountDescendants();
            }

            return total;
        }
    }
}