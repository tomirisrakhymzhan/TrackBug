using System;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class TicketMemberRepository : Repository<TicketMember>, ITicketMemberRepository
    {
        private ApplicationDbContext _db;

        public TicketMemberRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(TicketMember obj)
        {
            _db.TicketMembers.Update(obj);
        }
    }
}
