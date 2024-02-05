using Project.Models.AppModels;
using Project.Repositories.PostsRepository;

namespace Project.Services.PostsService
{
	public class PostsService : IPostsService
	{
        public IPostsRepository _postsRepository;

        public PostsService(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }


        public async Task<string> GetFirst3UsersPosts()
        {
            return await _postsRepository.GetFirst3UsersPosts();
        }

        public async Task<List<Posts>> GetAllPosts()
        {
            return await _postsRepository.GetAllAsync();
        }

        public async Task<bool> CreatePost(string DisName_or_Id, string text)
        {
            var user = await _postsRepository.GetUserProfile(DisName_or_Id);
            if (user == null)
                return false;
            var newPost = new Posts
            {
                UserProfileId = user.Id,
                Text = text
            };
            await _postsRepository.CreateAsync(newPost);
            await _postsRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdatePost(Guid postId, string text)
        {
            var post = await _postsRepository.FindByIdAsync(postId);
            if (post == null)
                return false;
            if(post.Text != text)
            {
                post.Text = text;
                post.DateModified = DateTime.UtcNow;
            }
            _postsRepository.Update(post);
            await _postsRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeletePost(Guid postId)
        {
            var post = await _postsRepository.FindByIdAsync(postId);
            if (post == null)
                return false;
            _postsRepository.Delete(post);
            await _postsRepository.SaveAsync();
            return true;
        }
    }
}

