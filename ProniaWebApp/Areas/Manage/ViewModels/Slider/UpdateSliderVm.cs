namespace ProniaWebApp.Areas.Manage.ViewModels.Slider
{
    public record UpdateSliderVm
    {
        public int Id { get; set; }
		public string UpTitle { get; set; }
		public string DownTitle { get; set; }
		public string Description { get; set; }
		public string? ImgUrl { get; set; }
		public IFormFile? Image { get; set; }
	}
}
