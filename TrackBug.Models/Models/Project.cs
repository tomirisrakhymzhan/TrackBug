using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrackBug.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(30)]
        public string ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Created Date-Time")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        //to record who created the project
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [Display(Name = "Created by")]
        public ApplicationUser ApplicationUser { get; set; }

        public int NumOfTickets { get; set; }
        public int NumOfClosedTickets { get; set; }
        //public int NumOfOpenTickets { get; set; }
        //public int NumOfMembers { get; set; }

        [NotMapped]
        public string CreatedDateTimeAsString { get; set; }
    }
}
