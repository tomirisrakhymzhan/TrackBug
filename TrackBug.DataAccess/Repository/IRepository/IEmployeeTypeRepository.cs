using System;
using TrackBug.Models;
namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IEmployeeTypeRepository : IRepository<EmployeeType>
    {
        void Update(EmployeeType obj);
    }
}
