using System;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        private ApplicationDbContext _db;

        public StatusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Status obj)
        {
            _db.Statuses.Update(obj);
        }
    }
}
