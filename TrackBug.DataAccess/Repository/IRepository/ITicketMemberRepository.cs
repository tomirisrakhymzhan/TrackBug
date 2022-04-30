using System;
using TrackBug.Models;

namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface ITicketMemberRepository : IRepository<TicketMember>
    {
        void Update(TicketMember obj);
    }
}
