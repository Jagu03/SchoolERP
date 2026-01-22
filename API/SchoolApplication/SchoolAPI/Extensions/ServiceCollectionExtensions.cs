using SchoolApplication.Interface;
using SchoolInfrastructure.Repositories;

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
            return services;
        }
    }
}
