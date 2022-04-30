using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackBug.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Job title")]
        public int EmployeeTypeId { get; set; }
        [ForeignKey("EmployeeTypeId")]
        [ValidateNever]
        public EmployeeType EmployeeType { get; set; }
    }
}
