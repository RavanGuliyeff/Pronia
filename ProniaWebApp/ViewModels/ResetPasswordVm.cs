namespace ProniaWebApp.ViewModels
{
	public record ResetPasswordVm
	{
        [DataType(DataType.Password), Required]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string? UserId { get; set; }
        public string? Token { get; set; }
    }
}
