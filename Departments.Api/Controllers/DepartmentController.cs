using Departments.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Departments.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentFileReaderService _departmentService;
        private readonly IConfiguration _config;

        public DepartmentsController(IDepartmentFileReaderService departmentService, IConfiguration config)
        {
            _departmentService = departmentService;
            _config = config;
        }

        [HttpGet("hierarchy")]
        public async Task<IActionResult> GetHierarchy()
        {
            string filePath = _config["DepartmentFiles:Path"] ?? string.Empty;
            var result = await _departmentService.ReadAllFilesAsync(filePath);
            return Ok(result);
        }
    }
}
