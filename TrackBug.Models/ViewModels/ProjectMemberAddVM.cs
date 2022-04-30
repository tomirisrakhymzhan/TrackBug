using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace TrackBug.Models.ViewModels
{
    public class ProjectMemberAddVM
    {
        public Project Project { get; set; }
        public ProjectMember ProjectMember { get; set; }
        public IEnumerable<SelectListItem> ApplicationUserList { get; set; }
        public string returnUrl { get; set; }
    }
}
