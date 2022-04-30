using System;
using TrackBug.Models;
namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface ITicketRepository: IRepository<Ticket>
    {
        void Update(Ticket obj);
    }
}
