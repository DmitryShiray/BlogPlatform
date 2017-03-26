using System;
using System.ComponentModel.DataAnnotations;

namespace BlogPlatform.ViewModels
{
    public class ProfileViewModel : BaseProfileViewModel
    {
        public DateTime RegistrationDate { get; set; }

        public bool IsCurrentUserProfile { get; set; }
    }
}
