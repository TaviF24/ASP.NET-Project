using Microsoft.AspNetCore.Identity;
using Project.Models.AppModels;

namespace Project.Models
{
	public class User : IdentityUser
	{
		public UserProfile userProfile { get; set; }
	}
}

