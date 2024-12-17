using System.ComponentModel.DataAnnotations;

namespace ProniaWebApp.ViewModels
{
    public record RegisterVm
    {
        [MinLength(3, ErrorMessage = "Name length must be higher than '3'")]
        public string Name { get; set; }

        [MinLength(3, ErrorMessage = "Surname length must be higher than '3'")]
        public string Surname { get; set; }

        [Required, MinLength(3)]
        public string Username { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]             
        public string ConfirmPassword { get; set; }
    }
}
