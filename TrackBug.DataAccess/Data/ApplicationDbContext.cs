using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackBug.Models;
namespace TrackBug.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMember> TicketMembers { get; set; }
        
    }
}
