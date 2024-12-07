namespace ProniaWebApp.ViewModels
{
	public record ForgetPasswordVm
	{
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
