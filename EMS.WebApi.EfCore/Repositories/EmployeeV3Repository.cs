using EMS.WebApi.EfCore.Data;
using EMS.WebApi.EfCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApi.EfCore.Repositories;

public class EmployeeV3Repository(EmsDbContext emsDbContext) : 
    BaseRepository<Employee>(emsDbContext), IEmployeeV3Repository
{
    public int UpdateCode(Employee employee)
    {
        return emsDbContext.Employee
                   .Where(e => e.Id == employee.Id)
                   .ExecuteUpdate(a => a.SetProperty(e => e.Code, e => employee.Code));
    }
}
