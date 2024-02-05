using Project.Models.AppModels;
using Project.Repositories.CommentsRepository;

namespace Project.Services.CommentsService
{
	public class CommentsService : ICommentsService
	{
		public ICommentsRepository _commentsRepository;

        public List<Comments> GetAllComm()
        {
            return _commentsRepository.GetComments();
        }

        public async Task<bool> CreateComment(string DisName_or_Id, Guid postId, string text)
        {
            var user = await _commentsRepository.GetUserProfile(DisName_or_Id);
            var post = await _commentsRepository.GetPost(postId);
            if (user == null || post ==null)
                return false;

            var newComm = new Comments
            {
                UserProfileId = user.Id,
                PostId = post.Id,
                Text = text
            };
            await _commentsRepository.CreateAsync(newComm);
            await _commentsRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateComm(Guid commId, string text)
        {
            var comm = await _commentsRepository.FindByIdAsync(commId);
            if (comm == null)
                return false;
            if (comm.Text != text)
            {
                comm.Text = text;
                comm.DateModified = DateTime.UtcNow;
            }
            _commentsRepository.Update(comm);
            await _commentsRepository.SaveAsync();
            return true;
        }


        public async Task<bool> DeleteComm(Guid commId)
        {
            var comm = await _commentsRepository.FindByIdAsync(commId);
            if (comm == null)
                return false;
            _commentsRepository.Delete(comm);
            await _commentsRepository.SaveAsync();
            return true;
        }
    }
}

