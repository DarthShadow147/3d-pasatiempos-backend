using _3d_pasatiempos_backend.Application.Dtos.CommonDto;
using _3d_pasatiempos_backend.Application.Dtos.ProjectDto;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using _3d_pasatiempos_backend.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _3d_pasatiempos_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _ProjectService;

        public ProjectController(IProjectService ProjectService)
        {
            _ProjectService = ProjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectRecords([FromQuery] QueryParams pQuery)
        {
            var lProjectData = await _ProjectService.GetPagedProjectAsync(pQuery);
            return Ok(lProjectData);
        }

        [HttpGet("{pProjectId}")]
        public async Task<IActionResult> GetProjectDetail(int pProjectId)
        {
            var lProjectData = await _ProjectService.GetProjectDetailAsync(pProjectId);
            return Ok(lProjectData);
        }

        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject([FromForm] CreateProjectDto pRequest,
            [FromServices] IValidator<CreateProjectDto> pValidator)
        {
            var lValidationTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidationTx.IsValid)
                return BadRequest(lValidationTx.Errors);

            var lCreateTx = await _ProjectService.CreateProjectAsync(pRequest);
            return Ok(lCreateTx);
        }

        [HttpPatch("UpdateProject")]
        public async Task<IActionResult> UpdateProject([FromForm] CreateProjectDto pRequest, [FromQuery] ProjectStatus pProjectStatus,
            [FromServices] IValidator<CreateProjectDto> pValidator)
        {
            var lValidationTx = await pValidator.ValidateAsync(pRequest);
            if (!lValidationTx.IsValid)
                return BadRequest(lValidationTx.Errors);

            await _ProjectService.UpdateProjectDetailAsync(pRequest, pProjectStatus);
            return NoContent();
        }
    }
}
