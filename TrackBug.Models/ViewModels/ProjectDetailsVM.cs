using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrackBug.Models.ViewModels
{
    public class ProjectDetailsVM
    {
        public Project Project { get; set; }
        public bool UserCanAdministerProject { get; set; }
        public IEnumerable<ProjectMember> ProjectMembers { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public string TicketPriority { get; set; }
        public string TicketStatus { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public int NumOpenTickets { get; set; }
        public int NumClosedTickets { get; set; }

    }
}