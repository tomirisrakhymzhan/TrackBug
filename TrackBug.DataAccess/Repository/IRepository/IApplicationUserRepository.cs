using System;
using TrackBug.Models;
namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
    }
}
