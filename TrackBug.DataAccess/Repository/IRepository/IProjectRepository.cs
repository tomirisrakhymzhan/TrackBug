using System;
using TrackBug.Models;
namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IProjectRepository:IRepository<Project>
    {
        void Update(Project obj);
    }
}
