using System;
using System.Collections.Generic;

namespace TrackBug.Models.ViewModels
{
    public class TicketVM
    {
        public Ticket Ticket { get; set; }
        public IEnumerable<TicketMember> TicketMembers { get; set; }
    }
}
