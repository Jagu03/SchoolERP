using SchoolApplication.Interface;
using SchoolApplication.Interface.YearlyEvents;
using SchoolInfrastructure.Repositories;
using SchoolInfrastructure.Repositories.YearlyEvents;

namespace SchoolAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSchoolAcademicsDependencies(this IServiceCollection services)
        {
            // Repository Dependencies
            services.AddScoped<IClassMasterRepository, ClassMasterRepository>();
            services.AddScoped<ISectionMasterRepository, SectionMasterRepository>();
            services.AddScoped<IClassSectionAllocationRepository, ClassSectionAllocationRepository>();
            services.AddScoped<ISubjectMasterRepository, SubjectMasterRepository>();
            services.AddScoped<IClassSectionSubjectMapRepository, ClassSectionSubjectMapRepository>();
            services.AddScoped<ISyllabusFinalizationRepository, SyllabusFinalizationRepository>();
            services.AddScoped<IStaffSubjectAllocationRepository, StaffSubjectAllocationRepository>();
            return services;
        }
    }
}
