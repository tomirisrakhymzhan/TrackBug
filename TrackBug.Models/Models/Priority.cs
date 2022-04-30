using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackBug.Models
{
    public class Priority
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [NotMapped]
        public string BadgeColor { get; set; }
    }
}
