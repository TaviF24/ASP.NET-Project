using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.PostsRepository
{
	public interface IPostsRepository: IGenericRepository<Posts>
    {
        public Task<string> GetFirst3UsersPosts();

    }
}

