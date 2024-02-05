using Project.Models.Base;

namespace Project.Models.AppModels
{
	public class Comments : BaseEntity
    {
        public string Text { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public UserProfile? UserProfile { get; set; }
        public Guid UserProfileId { get; set; }

        public Posts? Post { get; set; }
        public Guid PostId { get; set; }
    }
}

