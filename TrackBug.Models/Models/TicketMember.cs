using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrackBug.Models
{
    public class TicketMember
    {
        [Key]
        public int Id { get; set; }

        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        [ValidateNever]
        public Ticket Ticket { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [Display(Name = "Assigned to")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
