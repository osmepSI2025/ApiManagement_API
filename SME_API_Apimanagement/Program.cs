using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using SME_API_Apimanagement.Entities;
using SME_API_Apimanagement.Models;
using SME_API_Apimanagement.Repository;
using SME_API_Apimanagement.Service;
using SME_API_Apimanagement.Services;
using System;
using System.Text;

namespace SME_API_Apimanagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

       

            // Add Authentication Service
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "b7c7217e-ac1e-49d2-9a32-f1cfaae47218",
                        ValidAudience = "abebc109-fd69-4511-a3fa-184766cfa4da",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0a9dd1a5-c986-44a1-9f66-52afc094a621"))
                    };
                });


            // Add Quartz services
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("MOrganizationUpsertJob");
                q.AddJob<MOrganizationUpsertJob>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("MOrganizationUpsertJob-trigger")
                    .WithCronSchedule("0 0 * * * ?")); // Every day at midnight
            });
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);



            //add service 
            builder.Services.AddDbContext<ApiMangeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
           

            builder.Services.AddScoped<ITAPIMappingRepository, TAPIMappingRepository>();
            builder.Services.AddScoped<IMSystemRepository, MSystemRepository>();
            builder.Services.AddScoped<ITSystemAPIRepository, TSystemAPIRepository>();
            builder.Services.AddScoped<IDropdownRepository, DropdownRepository>();
            builder.Services.AddScoped<IMOrganizationRepository, MOrganizationRepository>();
            builder.Services.AddScoped<IMRegisterRepository, MRegisterRepository>();
            builder.Services.AddScoped<IErrorApiLogRepository, ErrorApiLogRepository>();
            builder.Services.AddScoped<UserManagementRepository>();
            builder.Services.AddScoped<UserManagementService>();
            builder.Services.AddScoped<HrEmployeeService>();
            builder.Services.AddScoped<ApiMappingService>();
            builder.Services.AddScoped<ITErrorApiLogRepository, TErrorApiLogRepository>();
     
          
            builder.Services.AddScoped<ITErrorApiLogService, TErrorApiLogService>();
            builder.Services.AddScoped<IMSystemInfoRepository, MSystemInfoRepository>();
            builder.Services.AddScoped<IMSystemInfoService, MSystemInfoService>();
            builder.Services.AddScoped< MOrganizationService>();

            builder.Services.AddScoped<IApiInformationRepository, ApiInformationRepository>();
            builder.Services.AddScoped<ICallAPIService, CallAPIService>(); // Register ICallAPIService with CallAPIService
            builder.Services.AddHttpClient<CallAPIService>();
            builder.Services.AddScoped<MailService>();
            // Add Swagger Configuration
            builder.Services.AddSwaggerGen(c =>
            {
                // Add Security Definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR...\""
                });

                // Add Security Requirement
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
