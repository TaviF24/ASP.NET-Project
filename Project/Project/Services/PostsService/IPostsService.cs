using Project.Models.AppModels;

namespace Project.Services.PostsService
{
	public interface IPostsService
	{
        public Task<string> GetFirst3UsersPosts();

        public Task<List<Posts>> GetAllPosts();

        public Task<bool> CreatePost(string DisName_or_Id, string text);

        public Task<bool> UpdatePost(Guid postId, string text);

        public Task<bool> DeletePost(Guid postId);

    }
}

