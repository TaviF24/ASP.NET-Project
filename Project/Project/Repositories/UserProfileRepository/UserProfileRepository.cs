using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.UserProfileRepository
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(AppDbContext appDbContext) : base(appDbContext) { }


        public async Task<List<Posts>> GetAllUserPosts_Join(Guid userId)
        {
            var user = await _table.FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                var posts_users = _appDbContext.Posts.Join(_appDbContext.UserProfiles, p => p.UserProfileId, u => u.Id,
                    (p, u) => new { p, u });//.Select(ob => ob.p).ToList();
                var res = from s in posts_users
                          where s.u.UserId == user.UserId
                          select s.p;

                return res.ToList();
            }
            return null;
        }

        public async Task<List<Comments>> GetAllUserComm_Include(Guid userId)
        {
            var user = await _table.FirstOrDefaultAsync(x => x.Id == userId);

            if(user != null)
            {
                var result = _appDbContext.Comments.Include(c => c.UserProfileId == user.Id).ToList();
                return result;
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

