using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.Metadatas
{
    public class TruckOwnerRatingMetadata
    {
        [Required]
        public int TruckOwnerId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}