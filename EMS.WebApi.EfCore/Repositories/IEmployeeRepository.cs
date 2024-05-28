using EMS.WebApi.EfCore.Models;
using System.Linq.Expressions;

namespace EMS.WebApi.EfCore.Repositories;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAll();
    Employee GetById(int id);
    int Create(Employee employee);
    int Update(Employee employee);
    int Delete(Employee employee);
    int DeleteById(int id);
    bool Exists(Expression<Func<Employee, bool>> predicate);

    int UpdateCode(Employee employee);
}
