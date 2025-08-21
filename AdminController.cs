using API_FINAL_PROJECT.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_FINAL_PROJECT.API.Admin.RequestObjects;
using AutoMapper;
using Domain.Services.Admin.Interfaces;
using Domain.Services.Admin.DTO_s;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace API_FINAL_PROJECT.API.Admin
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : BaseAPIController<AdminController>
    {
        private readonly IAdminService _adminService;
        private readonly IJobCategoryService _service;
        private readonly IMapper _mapper;
        public AdminController(IAdminService adminService, IJobCategoryService service, IMapper mapper)
        {
            _adminService = adminService;
            _service = service;
            _mapper = mapper;
        }

        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AdminLoginRequest request)
        {
            var loginDto = _mapper.Map<AdminLoginRequestDTO>(request);
            var result = await _adminService.LoginAsync(loginDto);
            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var adminId = GetAdminIdFromClaims();
            var profile = await _adminService.GetProfileAsync(adminId);
            if (profile == null)
                return NotFound("Admin not found");

            return Ok(profile);
        }

        [HttpPut("Updateprofile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] AdminProfileRequest request)
        {
            var adminId = GetAdminIdFromClaims();
            var dto = _mapper.Map<AdminProfileDTO>(request);
            await _adminService.UpdateProfileAsync(adminId, dto);
            return NoContent();
        }

       
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var adminId = GetAdminIdFromClaims();
            await _adminService.ChangePasswordAsync(adminId, request.NewPassword);
            return NoContent();
        }

        
        [HttpGet("ListAllJobs")]
        [Authorize]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _adminService.GetAllJobsAsync();
            return Ok(jobs);
        }

        
        [HttpGet("jobsByTitle")]
        [Authorize]
        public async Task<IActionResult> GetJobsByTitle([FromQuery] string title)
        {
            var jobs = await _adminService.GetJobsByTitleAsync(title);
            return Ok(jobs);
        }

        [HttpPut("jobs/approve/{jobId}")]
        [Authorize]
        public async Task<IActionResult> ApproveJob(Guid jobId)
        {
            await _adminService.ApproveJobAsync(jobId);
            return NoContent();
        }

        
        [HttpDelete("Deletejobs/{jobId}")]
        [Authorize]
        public async Task<IActionResult> DeleteJob(Guid jobId)
        {
            await _adminService.DeleteJobAsync(jobId);
            return NoContent();
        }

       
        [HttpGet("ListAllJobApplications")]
        [Authorize]
        public async Task<IActionResult> GetAllJobApplications()
        {
            var apps = await _adminService.GetAllJobApplicationsAsync();
            return Ok(apps);
        }

        private Guid GetAdminIdFromClaims()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
                throw new UnauthorizedAccessException("Admin ID not found in token.");

            return Guid.Parse(claim.Value); 
        }

        [HttpGet("GetAllJobCategory")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllCategoriesAsync();
            return Ok(result);
        }
        [Route("AddCategory")]
        [HttpPost]
        public async Task<IActionResult> Add(JobCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Job category name is required.");

            if (string.IsNullOrWhiteSpace(request.Description))
                return BadRequest("Job category description is required.");

            var dto = _mapper.Map<AddJobCategoryDTO>(request);
            var result = await _service.AddCategoryAsync(dto);

            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] JobCategoryUpdateRequest request)
        {
            var dto = _mapper.Map<UpdateJobCategoryDTO>(request);
            var result = await _service.UpdateCategoryAsync(dto);

            if (result == null)
                return NotFound("Category not found.");

            return Ok(result);
        }
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var success = await _service.DeleteCategoryAsync(id);
            if (!success)
                return NotFound("Category not found.");

            return NoContent();
        }
        [HttpPost]
        [Route("AddSkill")]
        public async Task<ActionResult> AddSkills(SkillRequest skillRequest)
        {
            var AddskillDto = _mapper.Map<AdminAddSkillDto>(skillRequest);
            await _adminService.CreateSkill(AddskillDto);
            return Ok(skillRequest);
        }

        [HttpPut]
        [Route("UpdateSkill({id})")]
        public async Task<ActionResult> UpdateSkill(Guid id, UpdateSkillRequest updateSkillRequest)
        {
            if (id != updateSkillRequest.Id)
            {
                return BadRequest();
            }
            var updateSkill = _mapper.Map<UpdateSkillDto>(updateSkillRequest);
            await _adminService.UpdateSkillAsync(updateSkill);
            return Ok(updateSkill);

        }

        [HttpDelete]
        [Route("DeleteSkill({id})")]
        public async Task<ActionResult> DeleteSkill(Guid id)
        {
            var deleted = await _adminService.DeleteSkillAsync(id);
            if (deleted == null) return NotFound();
            return Ok("Skill deleted sucessfully");
        }

        [HttpGet]
        [Route("GetAllSkills")]
        public async Task<ActionResult> GetAllSkill()
        {
            var allSkills = await _adminService.GetAllSkillAsync();
            return Ok(allSkills);
        }

        [HttpGet]
        [Route("GetSkillCount")]
        public IActionResult GetSkillCount()
        {
            var count = _adminService.GetSkillCount();
            return Ok(new { Count = count });
        }

        [HttpGet]
        [Route("GetJobSeekerCount")]
        public IActionResult GetJobSeekerCount()
        {
            var count = _adminService.GetJobSeekerCount();
            return Ok(new { Count = count });
        }

        [HttpDelete]
        [Route("DeleteJobSeeker({id})")]
        public async Task<ActionResult> DeleteJobSeeker(Guid id)
        {
            var seeker = await _adminService.DeleteJobSeekerAsync(id);
            if (seeker == null) return NotFound();
            return Ok("JobSeeker Deleted Successfully");
        }

        [HttpGet]
        [Route("GetAllJobSeekers")]
        public async Task<ActionResult> GetAllJobSeeker()
        {
            var jobSeekers = await _adminService.GetAllJobSeekerAsync();
            return Ok(jobSeekers);
        }

        [HttpGet]
        [Route("GetJobSeekerById({id})")]
        public async Task<ActionResult> GetJobSeekerById(Guid id)
        {
            var seeker = await _adminService.GetSeekerById(id);
            if (seeker == null) return NotFound();

            return Ok(seeker);
        }

    }
}
