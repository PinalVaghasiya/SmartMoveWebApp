using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Dtos
{
    public class LoginDto
    {
        [Required]
        public int LoginId { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public string UserType { get; set; }

        [Required]
        public bool EmailActivated { get; set; }

        public System.DateTime CreatedTime { get; set; }

        public Nullable<System.DateTime> ModifiedTime { get; set; }
    }
}