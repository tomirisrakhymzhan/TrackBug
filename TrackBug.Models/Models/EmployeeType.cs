using System;
using System.ComponentModel.DataAnnotations;

namespace TrackBug.Models
{
    public class EmployeeType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
