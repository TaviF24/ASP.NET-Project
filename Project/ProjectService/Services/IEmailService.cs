using ProjectService.Models;

namespace ProjectService.Services
{
	public interface IEmailService
	{
		void sendEmail(Message message);
	}
}

