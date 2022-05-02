using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrackBug.Models.ViewModels
{
    public class PriorityVM
    {
        public Priority Priority {get; set;}
        public IEnumerable<SelectListItem> BadgeColorsList { get; set; }
    
    }
}
