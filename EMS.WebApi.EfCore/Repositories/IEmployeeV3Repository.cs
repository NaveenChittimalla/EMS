using EMS.WebApi.EfCore.Models;
using System.Linq.Expressions;

namespace EMS.WebApi.EfCore.Repositories;

public interface IEmployeeV3Repository : IBaseRepository<Employee>
{
    int UpdateCode(Employee employee);
}
