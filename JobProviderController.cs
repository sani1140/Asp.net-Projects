using API_FINAL_PROJECT.API.Admin;
using API_FINAL_PROJECT.API.JobProvider.RequestObjects;
using API_FINAL_PROJECT.Controllers;
using AutoMapper;

using Domain.Services.Admin.Interfaces;
using Domain.Services.AuthUser.Interfaces;
using Domain.Services.JobProvider;
using Domain.Services.JobProvider.DTO_s;
using Domain.Services.JobProvider.Interfaces;
using Domain.Services.SystemUser.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Domain.Services.Job;
using Domain.Services.Job.DTO_s;
using Domain.Services.Job.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace API_FINAL_PROJECT.API.JobProvider
{
    
    [Route("careerx/api/v1/Jobprovider")]
    [ApiController]
    public class JobProviderController : BaseAPIController<JobProviderController>
    {


        private readonly IMapper _mapper;
        private readonly ISystemUserService _systemUserService;
        private readonly IJobPostService _jobService;
        private readonly IAuthUserService _authUserService;
        private readonly Domain.Services.JobProvider.Interfaces.IJobProviderService _service;

        public JobProviderController(IMapper mapper, ISystemUserService systemUserService, Domain.Services.JobProvider.Interfaces.IJobProviderService service, IJobPostService jobService, IAuthUserService authUserService)
        {
            _mapper = mapper;
            _systemUserService = systemUserService;
            _service = service;
            _jobService = jobService;
            _authUserService = authUserService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterJobProvider([FromBody] ProviderSignUpRequest request)
        {
            request.Id = Guid.NewGuid();


            var signUpDto = _mapper.Map<JobProviderRegisterRequestDTO>(request);


            await _systemUserService.CreateProviderSignUpRequest(signUpDto);

            return Ok(new
            {
                message = "Sign-up request submitted. Please verify your email.",
                signUpId = request.Id
            });
        }
        

        
        
        

        
        
        [HttpPost]
        [Route("Verify")]
        public async Task<IActionResult> VerifyEmail(Guid jobProviderRegisterRequestID,string password)
        {
            var isVerified=await _systemUserService.VerifyEmail(jobProviderRegisterRequestID, password);
            if (isVerified)
            {
                return Ok("Registered Successfully");
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Login")]

        public IActionResult Login(JobProviderLoginRequest request)
        {
            var user=_systemUserService.LoginJobProvider(request.Email, request.Password);
            if (user == null)
            {
                return BadRequest("Login failed");

            }
            return Ok(user);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _systemUserService.ResetPasswordAsync(request.Email, request.Password);

            if (!result)
                return BadRequest("User not found or password reset failed.");

            return Ok("Password updated successfully.");
        }

        [Authorize(Roles = "JOBPROVIDER")]
        [HttpPost("companyuser/create")]
        public async Task<IActionResult> CreateCompanyUser(CompanyProfileRequest request)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null) return Unauthorized("Invalid user context");

            var dto = _mapper.Map<CompanyUserDto>(request);
            await _service.CreateCompanyUser(Guid.Parse(userId), dto);
            return Ok("Company user created.");
        }

        [Authorize(Roles = "JOBPROVIDER")]
        [HttpPut("companyuser/update/{id}")]
        public async Task<IActionResult> UpdateCompanyUser(Guid id, CompanyProfileRequest request)
        {
            var dto = _mapper.Map<CompanyUserDto>(request);
            await _service.UpdateCompanyUser(id, dto);
            return Ok("Company user updated.");
        }

        [Authorize(Roles = "JOBPROVIDER")]
        [HttpDelete("companyuser/delete/{id}")]
        public async Task<IActionResult> DeleteCompanyUser(Guid id)
        {
            await _service.DeleteCompanyUser(id);
            return Ok("Company user deleted.");

        }
        [Authorize(Roles = "JOBPROVIDER")]
        [HttpGet("GetJobBY Id")]
        public async Task<IActionResult> GetJobById(Guid id)
        {
            var job = await _jobService.GetJobById(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }
        [HttpGet("GetAllJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return Ok(jobs);
        }
        [Authorize(Roles = "JOBPROVIDER")]
        [HttpPost("add-job")]
        public async Task<IActionResult> AddJob([FromBody] PostJobRequest request)
        {
            try
            {
                var dto = _mapper.Map<JobPostDTO>(request);


                var result = await _jobService.AddJobAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "JOBPROVIDER")]
        [HttpPut("Job Edit/JobId")]
        public async Task<IActionResult> UpdateJob(Guid id, PostJobRequest job)
        {
            try
            {
                var Job = _mapper.Map<JobPostDTO>(job);
                var updateJob = await _jobService.UpdateJobAsync(id, Job);
                return Ok(updateJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "JOBPROVIDER,ADMIN")]
        [HttpDelete("JobDelete/JobId")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            try
            {
                var deleted = await _jobService.DeleteJobAsync(id);
                if (!deleted) return NotFound($"Job With id{id} not found");
                return Ok("deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "JOBPROVIDER")]
        [HttpGet("GetAllApplication")]
        public async Task<IActionResult> GetAllApplications()
        {
            var applications = await _service.GetAllJobApplication();
            return Ok(applications);
        }

        [Authorize(Roles = "JOBPROVIDER")]
        [HttpGet("GetApplicationById")]
        public async Task<IActionResult> GetApplicationById(Guid id)
        {
            var applicantId = await _service.GetApplicationById(id);
            if (applicantId == null) return NotFound();
            return Ok(applicantId);
        }
        [Authorize(Roles = "JOBPROVIDER")]
        [HttpPost("filter resume by skill")]
        public async Task<IActionResult> GetFilterResume(JobFilterDTO filter)
        {

            var matches = await _service.GetAllMatchingProfiles(filter);
            return Ok(matches);
        }
        [Authorize(Roles = "JOBPROVIDER")]
        [HttpPost("search by  qualifications or workexperience")]

        public async Task<IActionResult> searchJobSeeker([FromBody] SearchDTO search)

        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var results = await _service.JobSeekerAsync(search);
            return Ok(results);
        }

        // GET https://localhost/careerx/api/v1/Jobprovider/getjobseeker-resume?id=GUID
        [HttpGet("getjobseeker-resume")]
        [Authorize(Roles = "JOBPROVIDER")]
        public async Task<IActionResult> GetResumes([FromQuery] Guid id)
        {
            

            var resume = await _service.GetResumeByIdAsync(id);
            if (resume == null) return NotFound();

            return Ok(new
            {
                resume.Id,
                resume.Title,
                resume.ResumeFile,
                resume.FileName,
                resume.JobSeekerProfileID
            });
        }


        // POST https://localhost/careerx/api/v1/Jobprovider/createinterviewschedule
        [HttpPost("createinterviewschedule")]
        [Authorize(Roles = "JOBPROVIDER")]
        public async Task<IActionResult> CreateInterview([FromBody] JobInterviewRequestDto scheduleDto)
        {
            var result = await _service.CreateInterviewScheduleAsync(scheduleDto);
            return Ok(result);
        }



        // GET https://localhost/careerx/api/v1/Jobprovider/getinterviewschedule?id=GUID
        [HttpGet("getinterviewschedule")]
        [Authorize(Roles = "JOBPROVIDER")]
        public async Task<IActionResult> GetInterviewById(Guid id)
        {
            try
            {
                var interviewDto = await _service.GetInterviewSchedulesByIdAsync(id);
                return Ok(interviewDto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }




       
        // PUT https://localhost/careerx/api/v1/Jobprovider/updateinterviewschedule?id=GUID

        [HttpPut("updateinterviewschedule")]
        [Authorize(Roles = "JOBPROVIDER")]
        public async Task<IActionResult> UpdateInterview([FromQuery] Guid id, [FromBody] JobInterviewRequestDto scheduleDto)
        {
            var updated = await _service.UpdateInterviewScheduleAsync(id, scheduleDto);

            var response = new JobInterviewResponseDto
            {
                Id = updated.Id,
                JobPost = updated.JobPost,
                JobSeeker = updated.JobSeeker,
                JobAppliation = updated.JobAppliation,
                CompanyUserId = updated.CompanyUserId,
                ScheduledOn = updated.ScheduledOn,
                Status = updated.Status,
                Location = updated.Location,
                Company = updated.Company
            };

            return Ok(response);
        }


        // DELETE https://localhost/careerx/api/v1/Jobprovider/deleteinterviewschedule?id=GUID
        [HttpDelete("deleteinterviewschedule")]
        [Authorize(Roles = "JOBPROVIDER")]
        public async Task<IActionResult> DeleteInterview([FromQuery] Guid id)
        {
            var result = await _service.DeleteInterviewScheduleAsync(id);
            return Ok("Deleted this Interview");
        }

        // POST https://localhost/careerx/api/v1/Jobprovider/logout
       

        [HttpPost]
        [Route("Logout")]
       [Authorize(Roles = "JOBPROVIDER")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            Guid jobProviderId = Guid.Parse(userIdClaim.Value);

            await _authUserService.LogoutAsync(jobProviderId);

            return Ok(new { message = "Logout successful. Please delete your token from your local/session storage." });

        }

            
      


    }
}

