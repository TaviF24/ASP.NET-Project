using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.UserProfileRepository
{
	public interface IUserProfileRepository : IGenericRepository<UserProfile>
	{
		/*
		  Folosirea metodelor din Linq: GroupBy, Where, etc; Folosirea Join si Include (1p)
		*/

		public Task<List<Posts>> GetAllUserPosts_Join(Guid userId);

		public Task<List<Comments>> GetAllUserComm_Include(Guid userId);

		public Task<string> CheckUser(string email);

        public Task<UserProfile> GetUserProfile(string Id_or_disName);

		//public Task<UserProfile> GetUserProfileByDy(string displayedName);

    }
}

