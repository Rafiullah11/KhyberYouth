﻿using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }

}
