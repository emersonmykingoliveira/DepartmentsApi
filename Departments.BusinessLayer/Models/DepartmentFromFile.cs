using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Models
{
    public class DepartmentFromFile
    {
        public int Oid { get; set; }
        public string? Title { get; set; }
        public string? Color { get; set; }
        public int? DepartmentParentOID { get; set; }
    }
}