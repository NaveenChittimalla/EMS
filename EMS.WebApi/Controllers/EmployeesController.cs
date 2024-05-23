using EMS.CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //api/employees -- All Employees
        [HttpGet()]
        public ActionResult<IEnumerable<Employee>> GetAll()
        {
            Employee db = new Employee();
            IEnumerable<Employee> employeeList = db.GetAll();
            return Ok(employeeList);
        }

        //api/employees/1
        [HttpGet("{id}")]
        public ActionResult<Employee> Get(int id)
        {
            Employee employee = new Employee();
            employee.Id = id;
            employee.Get();

            //if (employee.EmployeeCode == "" || employee.EmployeeCode == null)
            if (string.IsNullOrEmpty(employee.EmployeeCode))
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // api/employees
        [HttpPost()]
        public ActionResult Create(Employee employee)
        {
            if (employee.ExistsWithEmail())
            {
                ModelState.AddModelError("Email", "Employee already exists with email.");
            }
            if (employee.FirstName == employee.LastName)
            {
                ModelState.AddModelError("FirstName", "FirstName and LastName should be different.");
            }

            if (ModelState.IsValid)
            {
                employee.Create();
                return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
            }

            var validation = new ValidationProblemDetails(ModelState);
            validation.Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
            return BadRequest(validation);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Employee employee)
        {
            employee.Id = id;
            if (employee.Exists())
            {
                employee.Update();
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Employee employee = new Employee();
            employee.Id = id;

            if (employee.Exists())
            {
                employee.Delete();
                return NoContent();
            }
            return NotFound();
        }
    }
}
