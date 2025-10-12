using Departments.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentParser : IDepartmentParser
    {
        private readonly IFileSystem _fileSystem;

        public DepartmentParser(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<List<Department>> ParseFileAsync(string filePath)
        {
            var lines = await _fileSystem.File.ReadAllLinesAsync(filePath);
            var departments = new List<Department>();

            foreach (var (line, index) in lines.Skip(1).Select((v, i) => (v, i + 2)))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                departments.Add(ParseLine(line, index));
            }

            return departments;
        }

        private Department ParseLine(string line, int lineNumber)
        {
            var parts = line.Split(',');

            if (parts.Length < 3)
                throw new ArgumentException($"Line {lineNumber}: insufficient columns.");

            if (!int.TryParse(parts[0], out var oid))
                throw new ArgumentException($"Line {lineNumber}: invalid OID.");

            var title = parts[1];
            var color = parts[2];

            int parentOid = 0;
            if (parts.Length > 3 && int.TryParse(parts[3], out var pOid))
                parentOid = pOid;

            return new Department
            {
                Oid = oid,
                Title = title,
                Color = color,
                DepartmentParentOID = parentOid
            };
        }
    }
}
