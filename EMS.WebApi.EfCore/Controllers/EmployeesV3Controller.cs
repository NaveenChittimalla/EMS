using EMS.WebApi.EfCore.Models;
using EMS.WebApi.EfCore.ObjectResults;
using EMS.WebApi.EfCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.EfCore.Controllers;

[Route("api/v3/employees")]
[ApiController]
public class EmployeesV3Controller(IEmployeeV3Repository employeeRepository) : ControllerBase
{
    [HttpGet("")]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        var employeeList = employeeRepository.List();
        return Ok(employeeList);
    }

    [HttpGet("{id}")]
    public ActionResult<Employee> Get(int id)
    {
        var employee = employeeRepository.Find(id);
        if (employee is null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost("")]
    //[ModelValidationActionFilter]//automatic model validation with [ApiController] attribute on controller
    public ActionResult Post(Employee employee) //[FromBody] automatic model binding with [ApiController] attribute on controller
    {
        if (employeeRepository.Exists(e => e.Email.Equals(employee.Email))) 
        {
            ModelState.AddModelError("Email", "Employee already exists with this email");
        }
        if (!ModelState.IsValid)
        {
            return new BadRequestProblemValidationObjectResult(ModelState);
        }
        
        int rowsEffected = employeeRepository.Create(employee);
        if (rowsEffected > 0)
        {
            employee.Code = "EMS" + employee.Id;
            employeeRepository.UpdateCode(employee);
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

        if (!employeeRepository.Exists(e => e.Id.Equals(id)))
        {
            return NotFound();
        }
        employeeRepository.Update(employee);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Employee employee = employeeRepository.Find(id);
        if (employee is null)
        {
            return NotFound();
        }
        employeeRepository.Delete(employee);
        return NoContent();
    }
}
