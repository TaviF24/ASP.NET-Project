using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.PostsRepository
{
	public class PostsRepository : GenericRepository<Posts>, IPostsRepository
    {
        public PostsRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<string> GetFirst3UsersPosts()
        {
            var postsGrouped = await ( from s in _table
                                       orderby s.DateCreated
                                       group s by s.UserProfileId).ToListAsync();
            var author = string.Empty;
            string res = string.Empty;
            int cnt_usr = 3;
            foreach (var post in postsGrouped)
            {
                if (post == null)
                    continue;
                if (cnt_usr == 0)
                    break;
                cnt_usr--;
                string posts = string.Empty;
                int cnt_post = 3;

                var v = await _appDbContext.UserProfiles.FirstOrDefaultAsync(x => x.Id == post.Key);
                if (v != null)
                    author = v.DisplayedUsername;

                foreach (var p in post)
                {
                    if (cnt_post == 0)
                        break;

                    cnt_post--;
                    posts += "- "+p.Text + "\n\n";
                }
                res += $"Author : {author} \nPosts : \n{posts}\n\n";
            
            }

            return res;          
        }

        //public async Task<Posts> GetPostsById(Guid id)
        //{
        //    await _table.FirstOrDefaultAsync(x => x.)
        //}

        //public async Task<UserProfile> GetUserProfile(string Id_or_disName)
        //{
        //    var userProfile = await _appDbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == Id_or_disName || x.DisplayedUsername == Id_or_disName);
        //    if (userProfile == null)
        //        return null;
        //    return userProfile;
            
        //}

    }
}

