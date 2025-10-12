using Departments.BusinessLayer.Services;
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

        /// <summary>
        /// Get department hierarchy
        /// </summary>
        /// <remarks>
        /// Reads all department files from the configured file path and returns a complete hierarchy of departments.
        /// </remarks>
        /// <response code="200">Successfully retrieved the department hierarchy.</response>
        /// <response code="500">Internal server error while reading department files.</response>
        [HttpGet("hierarchy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHierarchy()
        {
            string filePath = _config["DepartmentFiles:Path"] ?? string.Empty;
            var result = await _departmentService.ReadAllFilesAsync(filePath);
            return Ok(result);
        }
    }
}
