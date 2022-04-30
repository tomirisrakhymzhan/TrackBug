using System;
using System.Threading.Tasks;

namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProjectRepository Project { get; }
        IEmployeeTypeRepository EmployeeType { get; }
        IPriorityRepository Priority { get; }
        IStatusRepository Status { get; }
        IProjectMemberRepository ProjectMember { get; }
        ITicketRepository Ticket { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ITicketMemberRepository TicketMember { get; }
        Task SaveAsync();
        void Save();
    }
}
