using System;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class EmployeeTypeRepository : Repository<EmployeeType>, IEmployeeTypeRepository
    {
        private ApplicationDbContext _db;

        public EmployeeTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(EmployeeType obj)
        {
            _db.EmployeeTypes.Update(obj);
        }
    }
}
