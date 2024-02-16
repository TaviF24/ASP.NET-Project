using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.AppModels;
using Project.Models.DTOs;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.UserProfileRepository
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(AppDbContext appDbContext) : base(appDbContext) { }


        public async Task<List<PostsDTO>> GetAllUserPosts_Join(Guid userId)
        {
            var user = await _table.FirstOrDefaultAsync(x => x.Id == userId);

            //if (user != null)
            //{
                var posts_users = await _appDbContext.Posts.Join(_appDbContext.UserProfiles, p => p.UserProfileId, u => u.Id,
                    (p, u) => new { p, u }).Where(s => s.u.UserId == user.UserId)
            .Select(s => new PostsDTO
            {
                Id = s.p.Id,
                Text = s.p.Text,
                
            })
            .ToListAsync();
                //.Select(ob => ob.p).ToList();
                //var res = from s in posts_users
                //          where s.u.UserId == user.UserId
                //          select s.p;
                //var l = await res.ToListAsync();
                //Console.WriteLine(res.ToList()[0]);
                return posts_users;
            //}
            //return null;
        }

        public async Task<List<CommentsDTO>> GetAllUserComm_Include(Guid userId)
        {
            var user = await _table.FirstOrDefaultAsync(x => x.Id == userId);
            
            if(user != null)
            {
                var comments = await _appDbContext.Comments
                .Include(c => c.UserProfile)
                .Where(c => c.UserProfile.UserId == user.UserId)
                .Select(c => new CommentsDTO
                {
                    Id = c.Id,
                    Text = c.Text,
                    // Map other properties as needed
                })
                .ToListAsync();

                return comments;

                ////var result = _table.Include(c => c.Posts.Where(x => x.Comments.Count==0));
                //var result = _appDbContext.Comments.Include(c => c.UserProfile);
                ///*
                //List<Comments> res = new List<Comments>();
                //foreach(var v in result)
                //{
                //    res.Add(v);
                //}
                //*/
                //return result.ToList();


            }

            return null;
        }

        public async Task<string>CheckUser(string email)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return string.Empty;
            return user.Id;
            //returneaza id-ul din Identity
        }

        public async Task<UserProfile> GetUserProfile(string Id_or_disName)
        {
            var userProfile = await _table.FirstOrDefaultAsync(x => x.UserId == Id_or_disName || x.DisplayedUsername == Id_or_disName);
            if (userProfile == null)
                return null;
            return userProfile;
            //obtin profilul prin displayedUsername/Id de la Identity
        }

        /*
        public Task<UserProfile> GetUserProfileByDysName(string displayedName)
        {
            var userProfile = await _table.FirstOrDefaultAsync(x => x.UserId == dis);
            if (userProfile == null)
                return null;
            return userProfile;
        }*/


    }
}

