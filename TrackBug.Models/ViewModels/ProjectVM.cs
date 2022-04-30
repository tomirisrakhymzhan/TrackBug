using System;
using System.Collections.Generic;

namespace TrackBug.Models.ViewModels
{
    public class ProjectVM
    {
        public Project Project { get; set; }
        public IEnumerable<ProjectMember> ProjectMembers { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
