namespace ProniaWebApp.Areas.Manage.ViewModels.Slider
{
    public record CreateSliderVm
    {
		public string UpTitle { get; set; }
		public string DownTitle { get; set; }
		public string Description { get; set; }
		public IFormFile Image {  get; set; }
	}
}
