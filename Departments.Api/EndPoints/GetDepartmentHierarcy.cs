using Departments.BusinessLayer.Models;

namespace Departments.Api.EndPoints
{
    public static partial class Endpoint
    {
        public static void GetDepartmentsHierarcy(this WebApplication app)
        {
            app.MapGet("DepartmentsHierarcy", async (IDepartmentFileReader departmentFileReader) =>
            {
                return await departmentFileReader.ReadAllFiles();
            });
        }
    }
}
