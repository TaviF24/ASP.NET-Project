using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.CommentsRepository
{
	public class CommentsRepository : GenericRepository<Comments>, ICommentsRepository
    {
        public CommentsRepository(AppDbContext appDbContext) : base(appDbContext) { }


        public async Task<Comments> GetCommentById(Guid id)
        {
            return await _table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Comments> GetComments()
        {
            return _table.ToList();
        }

        public async Task<Posts> GetPost(Guid Id)
        {
            var post =  await _appDbContext.Posts.FirstOrDefaultAsync(x=> x.Id == Id);
            if (post != null)
                return post;
            return null;
        }




        //public AppDbContext _appDbContext;

        //public CommentsRepository(AppDbContext appDbContext)
        //{
        //    _appDbContext = appDbContext;
        //}


        ////	Get all
        //public List<Comments> GetAll()
        //{
        //    return _appDbContext.Comments.ToList();
        //}

        //public async Task<List<Comments>> GetAllAsync()
        //{
        //    return await _appDbContext.Comments.ToListAsync();
        //}




        ////	Create
        //public void Create(Comments entity)
        //{
        //    _appDbContext.Comments.Add(entity);
        //}

        //public async Task CreateAsync(Comments entity)
        //{
        //    await _appDbContext.Comments.AddAsync(entity);
        //}

        //public void CreateRange(IEnumerable<Comments> entities)
        //{
        //    _appDbContext.Comments.AddRange(entities);
        //}

        //public async Task CreateRangeAsync(IEnumerable<Comments> entities)
        //{
        //    await _appDbContext.Comments.AddRangeAsync(entities);
        //}




        ////	Update
        //public void Update(Comments entity)
        //{
        //    _appDbContext.Comments.Update(entity);
        //}

        //public void UpdateRange(IEnumerable<Comments> entities)
        //{
        //    _appDbContext.Comments.UpdateRange(entities);
        //}




        ////	Delete
        //public void Delete(Comments entity)
        //{
        //    _appDbContext.Comments.Remove(entity);
        //}

        //public void DeleteRange(IEnumerable<Comments> entities)
        //{
        //    _appDbContext.Comments.RemoveRange(entities);
        //}




        ////	Find
        //public Comments FindById(Guid id)
        //{
        //    return _appDbContext.Comments.Find(id);
        //}

        //public async Task<Comments> FindByIdAsync(Guid id)
        //{
        //    return await _appDbContext.Comments.FindAsync(id);
        //}




        ////	Save
        //public bool Save()
        //{
        //    return _appDbContext.SaveChanges() > 0;
        //}

        //public async Task<bool> SaveAsync()
        //{
        //    return await _appDbContext.SaveChangesAsync() > 0;
        //}


        //public async Task<UserProfile> GetUserProfile(string Id_or_disName)
        //{
        //    var userProfile = await _appDbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == Id_or_disName || x.DisplayedUsername == Id_or_disName);
        //    if (userProfile == null)
        //        return null;
        //    return userProfile;

        //}

    }
}

