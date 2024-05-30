using EMS.WebApi.EfCore.Data;
using EMS.WebApi.EfCore.Models;
using EMS.WebApi.EfCore.ObjectResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace EMS.WebApi.EfCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(EmsDbContext emsDbContext) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("")]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        var employeeList = emsDbContext.Employee.ToList();
        return Ok(employeeList);
    }

    /// <summary>
    /// Get an employee by specified Id.
    /// </summary>
    /// <param name="id">Employee Id</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public ActionResult<Employee> Get(int id)
    {
        var employee = emsDbContext.Employee.Find(id);
        if (employee is null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [HttpPost("")]
    //[ModelValidationActionFilter]//automatic model validation with [ApiController] attribute on controller
    public ActionResult Post(Employee employee) //[FromBody] automatic model binding with [ApiController] attribute on controller
    {
        if (emsDbContext.Employee.Any(e => e.Email.Equals(employee.Email)))
        {
            ModelState.AddModelError("Email", "Employee already exists with this email");
        }
        if (!ModelState.IsValid)
        {
            return new BadRequestProblemValidationObjectResult(ModelState);
        }

        emsDbContext.Employee.Add(employee);
        int rowsEffected = emsDbContext.SaveChanges();
        if (rowsEffected > 0)
        {
            employee.Code = "EMS" + employee.Id;
            emsDbContext.Employee
                       .Where(e => e.Id == employee.Id)
                       .ExecuteUpdate(a => a.SetProperty(e => e.Code, e => employee.Code));
        }
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            ModelState.AddModelError("Id", "Id passed in URI not matched with body payload Id");
        }
        if (!ModelState.IsValid)
        {
            return new BadRequestProblemValidationObjectResult(ModelState);
        }

        if (!emsDbContext.Employee.Any(e => e.Id.Equals(id)))
        {
            return NotFound();
        }
        emsDbContext.Entry<Employee>(employee).State = EntityState.Modified;
        emsDbContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Employee employee = emsDbContext.Employee.Find(id);
        if (employee is null)
        {
            return NotFound();
        }
        emsDbContext.Employee.Remove(employee);
        emsDbContext.SaveChanges();
        return NoContent();
    }
}
