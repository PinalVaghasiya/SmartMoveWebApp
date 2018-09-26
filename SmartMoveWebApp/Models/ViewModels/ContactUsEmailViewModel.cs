using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.ViewModels
{
    public class ContactUsEmailViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(50)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(50)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(300)]
        public string Message { get; set; }
    }
}