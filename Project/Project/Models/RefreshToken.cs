using Project.Models.Base;

namespace Project.Models
{
	public class RefreshToken : BaseEntity
	{
		public required string Token { get; set; } = string.Empty;

		public DateTime Created { get; set; } = DateTime.Now;

		public DateTime Expires { get; set; }
	}
}

