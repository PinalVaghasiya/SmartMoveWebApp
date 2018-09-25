using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.ViewModels
{
    public class ContactUsEmailViewModel
    {
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public string Subject { get; set; }
        
        [Required]
        public string Message { get; set; }
    }
}