using System;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private ApplicationDbContext _db;

        public TicketRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Ticket obj)
        {
            _db.Tickets.Update(obj);
        }
    }
}
