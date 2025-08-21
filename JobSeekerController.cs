

﻿using System.Threading.Tasks;
using API_FINAL_PROJECT.API.Admin;
using API_FINAL_PROJECT.API.Jobseeker.RequestObjects;
using API_FINAL_PROJECT.Controllers;
using AutoMapper;
using Domain.Services.JobSeeker.DTO_s;
using Domain.Services.JobSeeker.Interfaces;
using Domain.Services.SystemUser.Interfaces;
using Microsoft.AspNetCore.Authorization;

﻿using Domain.Services.JobSeeker;






using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace API_FINAL_PROJECT.API.Jobseeker
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSeekerController : BaseAPIController<JobSeekerController>
    {


        private readonly IMapper _mapper;
        private readonly ISystemUserService _systemUserService;
        private readonly IJobSeekerService _jobSeekerService;
        public JobSeekerController(IMapper mapper, ISystemUserService systemUserService,IJobSeekerService jobSeekerService)
        {
            _mapper = mapper;
            _systemUserService = systemUserService;
            _jobSeekerService = jobSeekerService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterJobSeeker(API.Jobseeker.RequestObjects.JobSeekerSignUpRequest request)
        {
            var signupRequest = _mapper.Map<JobSeekerRegisterRequestDTO>(request);
            await _systemUserService.CreateSignUpRequest(signupRequest);
            return Ok(signupRequest);

        }
        [HttpPost]
        [Route("Verify")]
        public async Task<IActionResult> VerifyEmailAndSetPassword(Guid jobSeekerRegisterRequestID, string pass)
        {
            var isVerified = await _systemUserService.VerifyEmail_SetPass(jobSeekerRegisterRequestID, pass);
            if (isVerified)
            {
                return Ok("Registered Successfully");
            }
            return BadRequest();

        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(JobSeekerLoginRequest logdata)
        {

            var user = _systemUserService.LoginSeeker(logdata.Email, logdata.Password);

            if (user == null)
            {
                return BadRequest("Login Failed");
            }
            return Ok(user);
        }
        [HttpPatch]
        [Route("Change Password")]
        public async Task<IActionResult> ChangePassword(JobSeekerLoginRequest changePassRequest)
        {
            try
            {
                var changePassRequestDTO = _mapper.Map<PasswordChangeRequestDTO>(changePassRequest);
                var successMssg= await _systemUserService.ChangePass(changePassRequestDTO);
                return Ok(successMssg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        
        [HttpPost]
        [Route("Create Profile")]
        [Authorize(Roles = "JOBSEEKER")]
        public async Task<IActionResult> CreateProfile(CreateProfileRequest createProfileRequest)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
                var createProfileRequestDTO = _mapper.Map<ProfileDTO>(createProfileRequest);
                await _jobSeekerService.CreateProfile(userId, createProfileRequestDTO);

                return Ok("Created Profile!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPut]
        [Route("Edit Profile")]
        [Authorize(Roles = "JOBSEEKER")]
        public async Task<IActionResult> EditProfile(CreateProfileRequest editProfileRequest)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
                var editProfileRequestDTO = _mapper.Map<ProfileDTO>(editProfileRequest);
                await _jobSeekerService.UpdateProfile(userId, editProfileRequestDTO);
                return Ok(editProfileRequestDTO);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete]
        [Route("Delete Profile")]
        [Authorize(Roles ="JOBSEEKER")]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            await _jobSeekerService.DeleteProfile(userId);
            return Ok("Deleted Successfully");
        }
        [HttpPost]
        [Route("Upload Prof Pic")]
        [Authorize(Roles = "JOBSEEKER")]
        public async Task<IActionResult> UploadProfPic([FromBody] string Base64Image)
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            byte[] imageData = Convert.FromBase64String(Base64Image);
            await _jobSeekerService.UploadProfPic(imageData, userId);
            return Ok("Picture Uploaded!");
        }
        [HttpPatch]
        [Route("Update Prof Pic")]
        [Authorize(Roles = "JOBSEEKER")]
        public async Task<IActionResult> UpdateProfPic([FromBody] string Base64Image)
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            byte[] imageData = Convert.FromBase64String(Base64Image);
            await _jobSeekerService.UploadProfPic(imageData, userId);
            return Ok("Picture Updated!");
        }
        [HttpPost]
        [Route("Add Skills")]
        [Authorize(Roles = "JOBSEEKER")]
        public async Task<IActionResult> AddSkills([FromBody] Guid skillID )
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
                var skill = await _jobSeekerService.AddSkill(skillID, userId);
                return Ok(skill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            


        }
        

        

        //Add Skill to Profile
        [HttpPost("{profileId}/jobseeker-skills")]
        public async Task<IActionResult> AddJobSeekerSkill(Guid profileId, [FromBody] AddJobSeekerSkillRequest request)
        {
            await _jobSeekerService.AddJobSeekerSkillAsync(profileId, request.SkillId);
            return Ok("Skill added successfully.");
        }

        //Replace the Current Skill
        [HttpPut("jobseeker-skills/{id}")]
        public async Task<IActionResult> UpdateJobSeekerSkill(Guid id, [FromBody] AddJobSeekerSkillRequest request)
        {
            await _jobSeekerService.UpdateJobSeekerSkillAsync(id, request.SkillId);
            return Ok("Skill updated successfully.");
        }

        //Delete The Skill
        [HttpDelete("jobseeker-skills/{id}")]
        public async Task<IActionResult> DeleteJobSeekerSkill(Guid id)
        {
            await _jobSeekerService.DeleteJobSeekerSkillAsync(id);
            return Ok("Skill removed successfully.");
        }

        //Get the Skills of a jobseeker
        [HttpGet("{profileId}/jobseeker-skills")]
        public async Task<IActionResult> GetJobSeekerSkills(Guid profileId)
        {
            var skills = await _jobSeekerService.GetSkillsByProfileIdAsync(profileId);
            return Ok(skills);
        }



        //Add Work Experience to the Profile
        [HttpPost("work-experience")]
        public async Task<IActionResult> AddWorkExperience([FromBody] WorkExperienceRequest request)
        {
            var dto = new WorkExperienceDTO
            {
                JobTitle = request.JobTitle,
                CompanyName = request.CompanyName,
                Summary = request.Summary,
                ServiceStart = request.ServiceStart,
                ServiceEnd = request.ServiceEnd,
                JobSeekerProfileID = request.JobSeekerProfileID
            };

            await _jobSeekerService.AddWorkExperienceAsync(dto.JobSeekerProfileID, dto);
            return Ok("Work experience added successfully.");
        }

        // Update Work Experience
        [HttpPut("work-experience")]
        public async Task<IActionResult> UpdateWorkExperience([FromBody] WorkExperienceRequest request)
        {
            if (request.Id == null || request.Id == Guid.Empty)
                return BadRequest("Work experience Id is required for update.");

            var dto = new WorkExperienceDTO
            {
                Id = request.Id.Value,
                JobTitle = request.JobTitle,
                CompanyName = request.CompanyName,
                Summary = request.Summary,
                ServiceStart = request.ServiceStart,
                ServiceEnd = request.ServiceEnd,
                JobSeekerProfileID = request.JobSeekerProfileID
            };

            await _jobSeekerService.UpdateWorkExperienceAsync(dto);
            return Ok("Work experience updated successfully.");
        }

       // Delete Work Experience
        [HttpDelete("work-experience/{id}")]
        public async Task<IActionResult> DeleteWorkExperience(Guid id)
        {
            await _jobSeekerService.DeleteWorkExperienceAsync(id);
            return Ok("Work experience removed successfully.");
        }

        // List Work Experience by ProfileId
        [HttpGet("{profileId}/work-experience")]
        public async Task<IActionResult> GetWorkExperience(Guid profileId)
        {
            var experiences = await _jobSeekerService.GetWorkExperienceByProfileIdAsync(profileId);
            return Ok(experiences);
        }


        // Add Qualification to Profile
        [HttpPost("Qualification")]
        public async Task<IActionResult> AddQualification([FromBody] QualificationRequest request)
        {
            var dto = new QualificationDTO
            {
                CourseName = request.CourseName,
                Institution = request.Institution,
                Year = request.Year,
                JobSeekerProfileId = request.JobSeekerProfileId
            };
            await _jobSeekerService.AddQualificationAsync(dto);
            return Ok("Qualification added successfully.");
        }

        // Update the Qualification
        [HttpPut("Qualification")]
        public async Task<IActionResult> UpdateQualification([FromBody] QualificationRequest request)
        {
            if (request.Id == null || request.Id == Guid.Empty)
                return BadRequest("Qualification Id is required for update.");

            var dto = new QualificationDTO
            {
                Id = request.Id.Value,
                CourseName = request.CourseName,
                Institution = request.Institution,
                Year = request.Year,
                JobSeekerProfileId = request.JobSeekerProfileId
            };
            await _jobSeekerService.UpdateQualificationAsync(dto);
            return Ok("Qualification updated successfully.");
        }

        // Delete the Qualification
        [HttpDelete("Qualification/{id}")]
        public async Task<IActionResult> DeleteQualification(Guid id)
        {
            await _jobSeekerService.DeleteQualificationAsync(id);
            return Ok("Qualification removed successfully.");
        }

        // List the Qualification Of ProfileId
        [HttpGet("{profileId}/Qualifications")]
        public async Task<IActionResult> GetQualifications(Guid profileId)
        {
            var qualifications = await _jobSeekerService.GetQualificationsByProfileIdAsync(profileId);
            return Ok(qualifications);
        }


        //List all Jobs
        [HttpGet("list-jobs")]
        public async Task<IActionResult> ListJobs()
        {
            var jobs = await _jobSeekerService.ListJobsAsync();
            return Ok(jobs);
        }

        //Search Jobs By Title,Location
        [HttpGet("search-jobs")]
        public async Task<IActionResult> SearchJobs([FromQuery] string? title, [FromQuery] string? location)
        {
            var jobs = await _jobSeekerService.SearchJobsAsync(title, location);
            return Ok(jobs);
        }






        
        [HttpGet("Job/{jobId:guid}")]
        public async Task<IActionResult> GetJobDetailsById(Guid jobId)
        {
            
            var job = await _jobSeekerService.GetJobDetailsById(jobId);
            if (job == null)
            {
                return NotFound(new { message = "Job not found" });
            }
            return Ok(job);
        }
        [HttpPost("upload-resume")]
        public async Task<IActionResult> UploadResume([FromForm] UploadResumeRequest request)
        {
            var dto = new ResumeDTO
            {
                Title = request.Title,
                File = request.File,
                JobSeekerProfile = request.JobSeekerProfileID
            };

            await _jobSeekerService.UploadResumeAsync(dto);
            return Ok("Resume uploaded successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume(Guid id)
        {
            await _jobSeekerService.DeleteResumeAsync(id);
            return Ok("Resume deleted successfully.");
        }
        [HttpPost("apply-job")]
        public async Task<IActionResult> ApplyJob([FromBody] JobApplyDTO request)
        {
            await _jobSeekerService.ApplyJobAsync(request);
            return Ok("Job application submitted.");
        }
        [HttpGet("applied-jobs/{jobSeekerId}")]
        public async Task<IActionResult> GetAppliedJobs(Guid jobSeekerId)
        {
            var list = await _jobSeekerService.GetApplicationsByJobSeekerAsync(jobSeekerId);
            return Ok(list);
        }
        [HttpDelete("applied-jobs/{applicationId}")]
        public async Task<IActionResult> DeleteAppliedJob(Guid applicationId)
        {
            await _jobSeekerService.DeleteApplicationAsync(applicationId);
            return Ok("Application deleted.");
        }
        [HttpPost("save-job")]
        public async Task<IActionResult> SaveJob([FromBody] SaveJobDTO dto)
        {
            await _jobSeekerService.SaveJobAsync(dto);
            return Ok("Job saved successfully.");
        }

        [HttpGet("saved-jobs/{jobSeekerId}")]
        public async Task<IActionResult> GetSavedJobs(Guid jobSeekerId)
        {
            var savedJobs = await _jobSeekerService.GetSavedJobsAsync(jobSeekerId);
            return Ok(savedJobs);
        }

        [HttpDelete("delete-saved-job/{savedJobId}")]
        public async Task<IActionResult> DeleteSavedJob(Guid savedJobId)
        {
            await _jobSeekerService.DeleteSavedJobAsync(savedJobId);
            return Ok("Saved job deleted successfully.");
        }


    }
}
