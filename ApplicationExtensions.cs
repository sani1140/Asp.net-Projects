using Domain;
﻿using Domain.Services;
using Domain.Services.AuthUser.Interfaces;
using Domain.Services.AuthUser;
using Domain.Services.Email.Interfaces;
using Domain.Services.Email;
using Domain.Services.SystemUser.Interfaces;
using Domain.Services.SystemUser;

using Domain.Models;
using Domain.Services.Admin.Interfaces;
using Domain.Services.Admin;
using Microsoft.EntityFrameworkCore;
using Domain.Services.JobProvider;
using Domain.Services.JobProvider.Interfaces;

using Domain.Services.Job.Interfaces;
using Domain.Services.Job;

using Microsoft.Extensions.DependencyInjection;
using Domain.Services.JobSeeker.Interfaces;
using Domain.Services.JobSeeker;




using Domain.Services.Admin.Repository;
using Domain.Services.Admin.Services;

namespace API_FINAL_PROJECT.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(configuration.GetConnectionString("DEFAULT")));
            services.AddHttpContextAccessor();
            services.AddScoped<Domain.Services.JobProvider.Interfaces.IJobProviderRepository, Domain.Services.JobProvider.JobProviderRepository>();
            services.AddScoped<Domain.Services.JobProvider.Interfaces.IJobProviderService, Domain.Services.JobProvider.JobProviderService>();
            services.AddScoped<Domain.Services.Admin.Interfaces.IJobProviderRepository, Domain.Services.Admin.JobProviderRepository>();
            services.AddScoped<Domain.Services.Admin.Interfaces.IJobProviderService, Domain.Services.Admin.JobProviderService>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddTransient<IEmailService, EmailService>();

            services.AddScoped<ISystemUserRepository, SystemUserRepository>();
            services.AddScoped<ISystemUserService, SystemUserService>();
            services.AddScoped<IAuthUserRepository, AuthUserRepository>();
            services.AddScoped<IJobSeekerRepository,JobSeekerRepository>();
            services.AddScoped<IJobSeekerService, JobSeekerService>();
            services.AddScoped<IJobPostService, JobPostService>();
            services.AddScoped<IJobPostRepository, JobPostRepository>();
            services.AddScoped<IAuthUserService, AuthUserService>();
            
            

            services.AddScoped<IAdminRepository, AdminRepository>();
            
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
            services.AddScoped<IJobCategoryService, JobCategoryService>();
            
            return services;
            

        }
    }
}
