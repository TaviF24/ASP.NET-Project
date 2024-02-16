using Project.Models.AppModels;
using Project.Models.DTOs;

namespace Project.Services.UserProfileService
{
	public interface IUserProfileService
	{
        public Task<bool> CreateProfile(string Email, string DisplayedUserName, string? FirstName, string? LastName);

        public Task<List<PostsDTO>> GetUserPosts(string DisplayedUserName);

        public Task<List<CommentsDTO>> GetUserComments(string DisplayedUserName);

        public Task<bool> UpdateProfile(string DisName_or_Id, string newDisName, string newFirstName, string newLastName);

        public Task<bool> DeleteProfile(string DisName_or_Id);

    }
}

