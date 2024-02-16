using Project.Models.AppModels;
using Project.Models.DTOs;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.UserProfileRepository
{
	public interface IUserProfileRepository : IGenericRepository<UserProfile>
	{
		/*
		  Folosirea metodelor din Linq: GroupBy, Where, etc; Folosirea Join si Include (1p)
		*/

		public Task<List<PostsDTO>> GetAllUserPosts_Join(Guid userId);

		public Task<List<CommentsDTO>> GetAllUserComm_Include(Guid userId);

		public Task<string> CheckUser(string email);

        public Task<UserProfile> GetUserProfile(string Id_or_disName);

		//public Task<UserProfile> GetUserProfileByDy(string displayedName);

    }
}

