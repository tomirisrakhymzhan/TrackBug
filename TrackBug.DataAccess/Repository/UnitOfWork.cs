using System;
using System.Threading.Tasks;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            //Project = new ProjectRepository(_db);
            EmployeeType = new EmployeeTypeRepository(_db);
            Priority = new PriorityRepository(_db);
            Status = new StatusRepository(_db);
            //ProjectMember = new ProjectMemberRepository(_db);
            //Ticket = new TicketRepository(_db);
            //ApplicationUser = new ApplicationUserRepository(_db);
            //TicketMember = new TicketMemberRepository(_db);
        }

        //public IProjectRepository Project { get; private set; }
        public IEmployeeTypeRepository EmployeeType { get; private set; }
        public IPriorityRepository Priority { get; private set; }
        public IStatusRepository Status { get; private set; }
        //public IProjectMemberRepository ProjectMember { get; private set; }
        //public ITicketRepository Ticket { get; private set; }
        //public IApplicationUserRepository ApplicationUser { get; private set; }
        //public ITicketMemberRepository TicketMember { get; private set; }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
