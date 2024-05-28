using EMS.WebApi.EfCore.Data;
using EMS.WebApi.EfCore.Models;
using EMS.WebApi.EfCore.ObjectResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMS.WebApi.EfCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly EmsDbContext _emsDbContext;

    public EmployeesController(EmsDbContext emsDbContext)
    {
        _emsDbContext = emsDbContext;
    }

    [HttpGet("")]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        var employeeList = _emsDbContext.Employee.ToList();
        return Ok(employeeList);
    }

    [HttpGet("{id}")]
    public ActionResult<Employee> Get(int id)
    {
        var employee = _emsDbContext.Employee.Find(id);
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
        if (_emsDbContext.Employee.Any(e => e.Email.Equals(employee.Email))) 
        {
            ModelState.AddModelError("Email", "Employee already exists with this email");
        }
        if (!ModelState.IsValid)
        {
            return new BadRequestProblemValidationObjectResult(ModelState);
        }

        _emsDbContext.Employee.Add(employee);
        int rowsEffected = _emsDbContext.SaveChanges();
        if (rowsEffected > 0)
        {
            employee.Code = "EMS" + employee.Id;
            _emsDbContext.Employee
                       .Where(e => e.Id == employee.Id)
                       .ExecuteUpdate(a => a.SetProperty(e => e.Code, e => employee.Code));
        }
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }
}
