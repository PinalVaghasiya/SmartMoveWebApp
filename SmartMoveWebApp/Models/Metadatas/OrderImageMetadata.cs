using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.Metadatas
{
    public class OrderImageMetadata
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string ImageURL { get; set; }
    }
}