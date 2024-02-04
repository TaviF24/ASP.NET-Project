using System.ComponentModel.DataAnnotations;

namespace Project.Models.Authentication.SignUp
{
	public class ResetPasswordModel
	{
		[Required]
		public string? Password { get; set; }

		[Compare("Password", ErrorMessage = "The password and confirmation password does not match.")]
		public string? ConfirmPassword { get; set; }	

		public string? Email { get; set; }

        public string? Token { get; set; }
    }
}

