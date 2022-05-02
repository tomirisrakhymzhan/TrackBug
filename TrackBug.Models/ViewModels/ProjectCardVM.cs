using System;
namespace TrackBug.Models.ViewModels
{
    public class ProjectCardVM
    {
        public Project Project { get; set; }
        public int NumMembers { get; set; }
        public int NumOpenTickets { get; set; }
    }
}
