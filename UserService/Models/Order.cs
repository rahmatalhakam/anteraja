using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class Order
    {
        [Required]
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public double LatitudeStart { get; set; }
        [Required]
        public double LongitudeStart { get; set; }
        [Required]
        public double LatitudeEnd { get; set; }
        [Required]
        public double LongitudeEnd { get; set; }

    }
}