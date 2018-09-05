using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartMoveWebApp.Models.Metadatas
{
    public class TruckMetadata
    {
        [Required]
        public int TruckTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TruckMake { get; set; }

        [Required]
        [StringLength(50)]
        public string TruckModel { get; set; }

        [Required]
        [StringLength(50)]
        public string TruckYear { get; set; }

        [Required]
        [StringLength(50)]
        public string LicensePlate { get; set; }

        [Required]
        [StringLength(50)]
        public string TruckColor { get; set; }
    }
}