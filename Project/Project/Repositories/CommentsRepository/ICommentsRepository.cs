using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.CommentsRepository
{
	public interface ICommentsRepository : IGenericRepository<Comments>
    {

        public Task<Comments> GetCommentByCommId(Guid id);

        public List<Comments> GetComments();

        public Task<Posts> GetPost(Guid Id);

    }
}

