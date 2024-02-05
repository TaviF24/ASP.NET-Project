using Project.Models.Base;

namespace Project.Models.AppModels
{
	public class Posts : BaseEntity
    {
        public string Text { get; set; }

        //One to Many
        public UserProfile? UserProfile { get; set; }
        public Guid UserProfileId { get; set; }

        //Many to Many
        public ICollection<Comments> Comments { get; set; }

    }
}

