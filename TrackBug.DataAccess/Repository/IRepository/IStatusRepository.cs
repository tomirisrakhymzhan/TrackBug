using System;
using TrackBug.Models;
namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IStatusRepository : IRepository<Status>
    {
        void Update(Status obj);
    }
}
