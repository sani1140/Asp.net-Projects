
﻿using API_FINAL_PROJECT.API.Jobseeker.RequestObjects;
using API_FINAL_PROJECT.API.Admin.RequestObjects;
using AutoMapper;
using Domain.Models;
using Domain.Services.JobSeeker.DTO_s;

using Domain.Services.JobSeeker.Interfaces;


﻿using API_FINAL_PROJECT.API.JobProvider.RequestObjects;

using Domain.Services.JobProvider.DTO_s;


using Domain.Services.Job.DTO_s;
using Domain.Services.Admin.DTO_s;



namespace API_FINAL_PROJECT.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {


            CreateMap<API.Jobseeker.RequestObjects.JobSeekerSignUpRequest, JobSeekerRegisterRequestDTO>().ReverseMap();
            CreateMap<Domain.Models.JobSeekerSignUpRequest, JobSeekerRegisterRequestDTO>().ReverseMap();
            CreateMap<Domain.Models.JobSeekerSignUpRequest, SystemUser>().ReverseMap();
            CreateMap<AuthUser, SystemUser>().ReverseMap();
            CreateMap<JobSeekerLoginRequestDTO,AuthUser>().ReverseMap();    
            CreateMap<JobSeekerLoginRequest,PasswordChangeRequestDTO>().ReverseMap();
            CreateMap<CreateProfileRequest,ProfileDTO>().ReverseMap();
            CreateMap<JobSeekerProfile,ProfileDTO>().ReverseMap();
            CreateMap<JobSeekerSkill, JobSeekerSkillDto>().ReverseMap();

            CreateMap<AddJobSeekerSkillRequest, JobSeekerSkillDto>().ReverseMap(); 
            CreateMap<JobSeekerSkillDto, JobSeekerSkill>().ReverseMap();
            CreateMap<WorkExperienceRequest,Domain.Services.JobProvider.DTO_s.WorkExperienceDTO>().ReverseMap();
         
            CreateMap<QualificationRequest, QualificationDTO>();
            CreateMap<QualificationDTO, Qualification>().ReverseMap();



            CreateMap<ProviderSignUpRequest, JobProviderRegisterRequestDTO>().ReverseMap();
            CreateMap<JobProviderSignUpRequest, JobProviderRegisterRequestDTO>().ReverseMap();
            CreateMap<JobProviderRegisterRequestDTO, JobProviderSignUpRequest>().ReverseMap();

            // 2. DTO <-> SystemUser
            CreateMap<JobProviderRegisterRequestDTO, SystemUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<JobProviderSignUpRequest, SystemUser>().ReverseMap();




            // 3. DTO <-> Company
            CreateMap<JobProviderSignUpRequest, JobProviderCompany>()
                .ForMember(dest => dest.CompanyLegalName, opt => opt.MapFrom(src => src.CompanyLegalName))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Industry))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ReverseMap();

            // 4. Login
            CreateMap<JobProviderLoginRequestDTO, AuthUser>().ReverseMap();

            // 5. CompanyUser
            CreateMap<CompanyUser, CompanyUserDto>().ReverseMap();
            CreateMap<SystemUser, AuthUser>().ReverseMap();
            CreateMap<CompanyProfileRequest, CompanyUserDto>().ReverseMap();

            CreateMap<JobInterview, JobInterviewDto>()
           .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(d => d.JobTitle, opt => opt.MapFrom(src => src.JobPostID.JobTitle))
           .ForMember(d => d.CompanyName, opt => opt.MapFrom(src => src.CompanyID.ConcernedCompany.CompanyLegalName))
           // JobSeekerProfileName is already set in  service, no need to map here
           .ForMember(d => d.ScheduledOn, opt => opt.MapFrom(src => src.ScheduledOn))
           .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
           .ForMember(d => d.Location, opt => opt.MapFrom(src => src.Location));




            CreateMap<PostJobRequest,Domain.Services.Job.DTO_s.JobPostDTO>().ReverseMap();
            CreateMap<JobPost, Domain.Services.Job.DTO_s.JobPostDTO>().ReverseMap();

            // DTO -> Entity
            CreateMap<Domain.Services.Job.DTO_s.JobPostDTO, JobPost>()
                .ForMember(dest => dest.Skills, opt => opt.Ignore())
                .ForMember(dest => dest.Responsibilities, opt => opt.Ignore()
                );
            // Entity -> DTO
            CreateMap<JobPost, Domain.Services.Job.DTO_s.JobPostDTO>()
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skills.Select(s => s.Name)))
                .ForMember(dest => dest.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.Select(r => r.Name)))
                .ForMember(dest => dest.JobCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.JobProviderCompanyId, opt => opt.MapFrom(src => src.JobProviderCompany.Id));
            ////DTo To Entity
            //CreateMap<JobApplication, JobApplicationDTO>()
            //    .ForMember(dest => dest.JobSeekerId, opt => opt.MapFrom(src => src.JobSeeker))
            //    .ForMember(dest => dest.JobSeekerName, opt => opt.MapFrom(src => src.ApplicantID))
            //     .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.Resume))
            //      .ForMember(dest => dest.ResumeTitle, opt => opt.MapFrom(src => src.ResumeID.Title))
            //       .ForMember(dest => dest.JobPostId, opt => opt.MapFrom(src => src.JobPost))
            //        .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPostID.JobTitle))
            //         .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<JobSeekerProfile, MatchingProfileDTO>();
            CreateMap<Resume, MatchingProfileDTO>();
            // Configure AutoMapper
            CreateMap<JobSeekerProfile, MatchingProfileDTO>()
                .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.ProfileName));
            CreateMap<JobSeekerProfile, JobSeekerProfileDTO>()
                .ForMember(dest => dest.JobseekerID, opt => opt.MapFrom(src => src.JobSeeker))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ProfileName))
                .ForMember(dest => dest.Qualifications, opt => opt.MapFrom(src => src.Qualifications))
                .ForMember(dest => dest.WorkExperiences, opt => opt.MapFrom(src => src.WorkExperiences));







            CreateMap<JobPost, JobSeekerJobPostDTO>().ReverseMap();
            CreateMap<JobSeekerJobPostDTO, JobApplication>().ReverseMap();
            CreateMap<AdminLoginRequest, AdminLoginRequestDTO>();
            CreateMap<AdminProfileRequest, AdminProfileDTO>();
            CreateMap<SkillRequest, AdminAddSkillDto>().ReverseMap();
            CreateMap<AdminAddSkillDto, Skill>().ReverseMap();
            CreateMap<UpdateSkillDto, UpdateSkillRequest>().ReverseMap();
            CreateMap<UpdateSkillDto, Skill>().ReverseMap();
            CreateMap<GetJobSeekerDto, Jobseeker>().ReverseMap();
            CreateMap<JobCategoryRequest, AddJobCategoryDTO>();
            CreateMap<JobCategoryUpdateRequest, UpdateJobCategoryDTO>();
            CreateMap<JobCategory, AddJobCategoryDTO>().ReverseMap();
            CreateMap<JobCategoryRequest, JobCategory>().ReverseMap();
            CreateMap<AddJobCategoryDTO, JobCategory>();
        }


        }
    }
   
    


