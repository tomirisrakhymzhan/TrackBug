using System;
using TrackBug.Models;
namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IPriorityRepository : IRepository<Priority>
    {
        void Update(Priority obj);
    }
}
