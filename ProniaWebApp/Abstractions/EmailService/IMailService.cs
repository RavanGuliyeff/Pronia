
namespace ProniaWebApp.Abstractions.EmailService
{
	public interface IMailService
	{
		Task SendEmailAsync(MailRequest mailRequest);
	}
}
