using System;
using TrackBug.Models;
using TrackBug.DataAccess.Repository.IRepository;

namespace TrackBug.DataAccess.Repository
{
    public class ProjectMemberRepository : Repository<ProjectMember>, IProjectMemberRepository
    {
        private ApplicationDbContext _db;

        public ProjectMemberRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(ProjectMember obj)
        {
            _db.ProjectMembers.Update(obj);
        }
    }
}
