using Project.Models.Base;

namespace Project.Models.AppModels
{
	public class UserProfile : BaseEntity
	{
		public string? FirstName { get; set; } = string.Empty;

		public string? LastName { get; set; } = string.Empty;

		public string DisplayedUsername { get; set; }

		//One to One
		public User User { get; set; }
		public string UserId { get; set; }

		//One to Many
		public ICollection<Posts> Posts { get; set; }

        //Many to Many
        public ICollection<Comments> Comments { get; set; }

    }
}

