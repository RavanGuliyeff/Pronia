namespace ProniaWebApp.ViewModels
{
	public record ConfirmEmailVm
	{
		[Required]
        public string Key { get; set; }
    }
}
