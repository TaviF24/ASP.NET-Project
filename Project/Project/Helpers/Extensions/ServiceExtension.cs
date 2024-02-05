using Project.Repositories.UserProfileRepository;
using Project.Services.UserProfileService;

namespace Project.Helpers.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            //services.AddTransient<IStudentRepository, StudentRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserProfileService, UserProfileService>();
            return services;
        }
    }
    
}

