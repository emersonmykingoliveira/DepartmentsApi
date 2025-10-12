using Departments.BusinessLayer.Models;

namespace Departments.Api.EndPoints
{
    public static partial class Endpoint
    {
        public static void GetDepartmentsHierarcy(this WebApplication app)
        {
            app.MapGet("DepartmentsHierarcy", async (IDepartmentFileReaderService departmentFileReader, IConfiguration config) =>
            {
                string filePath = config["DepartmentFiles:Path"] ?? string.Empty;
                return await departmentFileReader.ReadAllFilesAsync(filePath);
            });
        }
    }
}
