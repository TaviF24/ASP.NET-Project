using System.ComponentModel.DataAnnotations;

namespace Project.Models.Authentification.Login
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Username or Email is required")]
		public string? Username_or_Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}

