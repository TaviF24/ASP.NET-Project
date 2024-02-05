using Project.Data;
using Project.Models.AppModels;
using Project.Repositories.UserProfileRepository;

namespace Project.Services.UserProfileService
{
	public class UserProfileService : IUserProfileService
	{
        public IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<bool> CreateProfile(string email, string DisplayedUserName, string? FirstName, string? LastName)
        {
            var userId = await _userProfileRepository.CheckUser(email);
            if (userId == string.Empty)
                return false;

            var userProfile = await _userProfileRepository.GetUserProfile(userId);
            if (userProfile != null)
                return false;

            
            
            var newProfile = new UserProfile
            {
                FirstName = FirstName,
                LastName = LastName,
                DisplayedUsername = DisplayedUserName,
                UserId = userId
            };

            await _userProfileRepository.CreateAsync(newProfile);
            await _userProfileRepository.SaveAsync();
            
            return true;
        }

        public async Task<List<Posts>> GetUserPosts(string Username)
        {
            var user = await _userProfileRepository.GetUserProfile(Username);
            if(user != null)
                return await _userProfileRepository.GetAllUserPosts_Join(user.Id);
            return null;
        }

        public async Task<List<Comments>> GetUserComments(string Username)
        {
            var user = await _userProfileRepository.GetUserProfile(Username);
            if (user != null)
                return await _userProfileRepository.GetAllUserComm_Include(user.Id);
            return null;
        }

        public async Task<bool> UpdateProfile(string DisName_or_Id, string newDisName, string? newFirstName, string? newLastName)
        {
            var user = await _userProfileRepository.GetUserProfile(DisName_or_Id);
            if (user == null)
                return false;
            if (newDisName != user.DisplayedUsername)
                user.DisplayedUsername = newDisName;
            if (newFirstName != user.FirstName)
                user.FirstName = newFirstName;
            if (newLastName != user.LastName)
                user.LastName = newLastName;
            _userProfileRepository.Update(user);
            await _userProfileRepository.SaveAsync();

            return true;
        }

    }
}

