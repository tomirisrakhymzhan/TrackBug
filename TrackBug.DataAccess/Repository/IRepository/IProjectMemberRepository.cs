using System;
using TrackBug.Models;

namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IProjectMemberRepository:IRepository<ProjectMember>
    {
        void Update(ProjectMember obj);
    }
}
