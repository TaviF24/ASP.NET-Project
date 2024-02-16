using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.AppModels;
using Project.Models.Base;


namespace Project.Repositories.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext? _appDbContext;
        protected readonly DbSet<TEntity>? _table;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _table = _appDbContext.Set<TEntity>();  
        }




        //	Get all
        public List<TEntity> GetAll()
        {
            return _table.AsNoTracking().ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _table.AsNoTracking().ToListAsync();
        }




        //	Create
        public void Create(TEntity entity)
        {
            _table.Add(entity);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _table.AddAsync(entity);
        }

        public void CreateRange(IEnumerable<TEntity> entities)
        {
            _table.AddRange(entities);
        }

        public async Task CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            await _table.AddRangeAsync(entities);
        }




        //	Update
        public void Update(TEntity entity)
        {
            _table.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _table.UpdateRange(entities);
        }




        //	Delete
        public void Delete(TEntity entity)
        {
            _table.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _table.RemoveRange(entities);
        }




        //	Find
        public TEntity FindById(Guid id)
        {
            return _table.Find(id);		
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _table.FindAsync(id);
        }




        //	Save
        public bool Save()
        {
            return _appDbContext.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }


        public async Task<UserProfile> GetUserProfile(string Id_or_disName)
        {
            var userProfile = await _appDbContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == Id_or_disName || x.DisplayedUsername == Id_or_disName);
            if (userProfile == null)
                return null;
            return userProfile;

        }

        //public async Task<Comments> GetCommentsByCommIdAsync(Guid Id)
        //{
        //    return await _appDbContext.Comments.FirstOrDefaultAsync(comm => comm.Id == Id);
        //}

    }
}

