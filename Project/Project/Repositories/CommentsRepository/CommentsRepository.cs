using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.CommentsRepository
{
	public class CommentsRepository : GenericRepository<Comments>, ICommentsRepository
    {
        public CommentsRepository(AppDbContext appDbContext) : base(appDbContext) { }


        public async Task<Comments> GetCommentByCommId(Guid id)
        {
            return await _table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Comments> GetComments()
        {
            return _table.ToList();
        }

        public async Task<Posts> GetPost(Guid Id)
        {
            Posts post =  await _appDbContext.Posts.FirstOrDefaultAsync(x=> x.Id == Id);
            if (post != null)
                return post;
            return null;
        }

    }
}

