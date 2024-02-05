using Project.Models.AppModels;
using Project.Repositories.GenericRepository;

namespace Project.Repositories.CommentsRepository
{
	public interface ICommentsRepository : IGenericRepository<Comments>
    {

        public Task<Comments> GetCommentById(Guid id);

        public List<Comments> GetComments();

        public Task<Posts> GetPost(Guid Id);

        ////	Create
        //void Create(Comments entity);

        //Task CreateAsync(Comments entity);

        //void CreateRange(IEnumerable<Comments> entities);

        //Task CreateRangeAsync(IEnumerable<Comments> entities);




        ////	Update
        //void Update(Comments entity);

        //void UpdateRange(IEnumerable<Comments> entities);




        ////	Delete
        //void Delete(Comments entity);

        //void DeleteRange(IEnumerable<Comments> entities);




        ////	Find
        //Comments FindById(Guid id);

        //Task<Comments> FindByIdAsync(Guid id);




        ////	Save
        //bool Save();

        //Task<bool> SaveAsync();


        //public Task<UserProfile> GetUserProfile(string Id_or_disName);
    }
}

