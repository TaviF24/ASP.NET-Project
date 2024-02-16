using Project.Data;

namespace Project.Repositories.AdminRepository
{
	public class AdminRepository : IAdminRepository
	{
        protected readonly AppDbContext? _appDbContext;

        public AdminRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }





    }
}

