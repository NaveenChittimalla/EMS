using EMS.WebApi.EfCore.Data;
using EMS.WebApi.EfCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMS.WebApi.EfCore.Repositories;

public class EmployeeRepository(EmsDbContext emsDbContext) : IEmployeeRepository
{
    public IEnumerable<Employee> GetAll()
    {
        return emsDbContext.Employee.AsNoTracking().ToList();
    }

    public Employee GetById(int id)
    {
        return emsDbContext.Employee.AsNoTracking().FirstOrDefault(e => e.Id.Equals(id));
    }

    public int Create(Employee employee)
    {
        emsDbContext.Employee.Add(employee);
        return emsDbContext.SaveChanges();
    }

    public int Update(Employee employee)
    {
        emsDbContext.Entry<Employee>(employee).State = EntityState.Modified;
        return emsDbContext.SaveChanges();
    }

    public int Delete(Employee employee)
    {
        emsDbContext.Employee.Remove(employee);
        return emsDbContext.SaveChanges();
    }

    public int DeleteById(int id)
    {
        emsDbContext.Employee.Remove(new Employee { Id = id });
        return emsDbContext.SaveChanges();
    }

    public bool Exists(Expression<Func<Employee, bool>> predicate)
    {
        return emsDbContext.Employee.AsNoTracking().Any(predicate);
    }

    public int UpdateCode(Employee employee)
    {
        return emsDbContext.Employee
                   .Where(e => e.Id == employee.Id)
                   .ExecuteUpdate(a => a.SetProperty(e => e.Code, e => employee.Code));
    }
}
