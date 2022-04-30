using System;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class PriorityRepository : Repository<Priority>, IPriorityRepository
    {
        private ApplicationDbContext _db;

        public PriorityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Priority obj)
        {
            _db.Priorities.Update(obj);
        }
    }
}
