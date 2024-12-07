namespace ProniaWebApp.Helpers.Email
{
	public record MailRequest
	{
		public string ToEmail { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public List<IFormFile> Attachments { get; set; }
	}
}
