using System;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private ApplicationDbContext _db;

        public ProjectRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Project obj)
        {
            _db.Projects.Update(obj);
        }
    }
}
