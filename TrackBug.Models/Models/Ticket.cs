using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrackBug.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Created Date-Time")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        [Display(Name = "Due Date-Time")]
        [DataType(DataType.DateTime)]
        public DateTime DueDateTime { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public int PriorityID { get; set; }
        [ForeignKey("PriorityID")]
        [ValidateNever]
        public Priority Priority { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusID { get; set; }
        [ForeignKey("StatusID")]
        [ValidateNever]
        public Status Status { get; set; }

        //to record who created the ticket
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [Display(Name = "Created by")]
        public ApplicationUser ApplicationUser { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]
        public Project Project { get; set; }

        [NotMapped]
        public string CreatedDateTimeAsString { get; set; }
        [NotMapped]
        public string DueDateTimeAsString { get; set; }

    }
}
