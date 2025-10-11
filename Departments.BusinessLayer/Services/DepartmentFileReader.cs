using Departments.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Departments.BusinessLayer.Services
{
    public class DepartmentFileReader : IDepartmentFileReader
    {
        public string FilePath { get; set; }

        public DepartmentFileReader(string filePath)
        {
            FilePath = filePath;
        }

        public async Task<string> ReadAllFiles()
        {
            string[] files = Directory.GetFiles(FilePath);

            foreach (string file in files)
            {
                string[] content = await File.ReadAllLinesAsync(file);

                foreach (string line in content)
                {

                }
            }

            return string.Empty;
        }
    }
}
