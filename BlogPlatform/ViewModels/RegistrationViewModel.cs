using System.ComponentModel.DataAnnotations;

namespace BlogPlatform.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", ErrorMessage = "E-mail address is not valid")]
        public string EmailAddress { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
