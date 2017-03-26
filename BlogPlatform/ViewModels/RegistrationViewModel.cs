using System.ComponentModel.DataAnnotations;

namespace BlogPlatform.ViewModels
{
    public class RegistrationViewModel : BaseProfileViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
