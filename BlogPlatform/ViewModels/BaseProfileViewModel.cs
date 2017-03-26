﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BlogPlatform.ViewModels
{
    public abstract class BaseProfileViewModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", ErrorMessage = "E-mail address is not valid")]
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Nickname { get; set; }
    }
}
