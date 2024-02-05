using Project.Repositories.CommentsRepository;
using Project.Repositories.PostsRepository;
using Project.Repositories.UserProfileRepository;
using Project.Services.CommentsService;
using Project.Services.PostsService;
using Project.Services.UserProfileService;

namespace Project.Helpers.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<IPostsRepository, PostsRepository>();
            services.AddTransient<ICommentsRepository, CommentsRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<ICommentsService, CommentsService>();
            return services;
        }
    }
    
}

