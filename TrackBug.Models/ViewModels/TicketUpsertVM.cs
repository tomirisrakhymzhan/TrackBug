using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrackBug.Models.ViewModels
{
    public class TicketUpsertVM
    {
        public Ticket Ticket {get; set;}
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
    
    }
}
