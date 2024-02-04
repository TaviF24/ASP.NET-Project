using Project.Models.Email;

namespace Project.Services.EmailService
{
	public interface IEmailService
	{       
         void sendEmail(Message message);
    }
}

