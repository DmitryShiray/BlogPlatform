using System;
using System.ComponentModel.DataAnnotations;

namespace BlogPlatform.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", ErrorMessage = "E-mail address is not valid")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
