using System;
using System.ComponentModel.DataAnnotations;

namespace TrackBug.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
