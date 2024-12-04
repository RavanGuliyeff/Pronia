namespace ProniaWebApp.ViewModels
{
    public record LoginVm
    {
        [Required]
        public string EmailOrUsername { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
