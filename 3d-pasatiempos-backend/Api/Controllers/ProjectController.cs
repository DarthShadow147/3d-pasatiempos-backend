using _3d_pasatiempos_backend.Application.Dtos.Project;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _Service;

        public ProjectController(IProjectService Service)
        {
            _Service = Service;
        }

        [HttpGet("GetProjectResume")]
        public async Task<IActionResult> GetProjectResume([FromQuery] List<string> pStatus = null)
        {
            try
            {
                var lResult = await _Service.GetAllAsync(pStatus);
                return Ok(lResult);
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }

        [HttpGet("{pProjectId}")]
        public async Task<IActionResult> GetProjectDetail(int pProjectId)
        {
            try
            {
                var lResult = await _Service.GetDetailByIdAsync(pProjectId);
                return Ok(lResult);
            }
            catch (Exception lEx)
            {
                return NotFound(lEx.Message);
            }
        }

        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest pRequest)
        {
            try
            {
                var lResult = await _Service.CreateProjectAsync(pRequest);
                if (lResult)
                    return Ok(lResult);
                else
                    return BadRequest("Error to generate project");
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }

        [HttpPut("{pProjectId}/change")]
        public async Task<IActionResult> UpdateProject(int pProjectId, [FromQuery] string pStatus)
        {
            try
            {
                await _Service.UpdateStatusAsync(pProjectId, pStatus);
                return NoContent();
            }
            catch (Exception lEx)
            {
                return BadRequest(lEx.Message);
            }
        }

    }
}
