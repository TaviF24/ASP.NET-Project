using Project.Models.AppModels;

namespace Project.Services.CommentsService
{
	public interface ICommentsService
	{
        public List<Comments> GetAllComm();


        public  Task<bool> CreateComment(string DisName_or_Id, Guid postId, string text);


        public Task<bool> UpdateComm(Guid commId, string text)
        ;


        public Task<bool> DeleteComm(Guid commId)
        ;
    }
}

