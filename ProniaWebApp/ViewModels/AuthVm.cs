namespace ProniaWebApp.ViewModels
{
    public record AuthVm
    {
        public LoginVm Login { get; set; }
        public RegisterVm Register { get; set; }
    }
}
