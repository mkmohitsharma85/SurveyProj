﻿using System.ComponentModel.DataAnnotations;

namespace Survey.Contract.Model.Request
{
    public class UserCreationRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        public List<string> Roles { get; set; }
    }
}
