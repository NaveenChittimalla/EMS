using EMS.CoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.Controllers
{
    [Route("api/v2/employees")]
    [ApiController]
    public class EmployeesV2Controller : ControllerBase
    {
        // api/v2/employees -- All Employees
        [HttpGet()]
        public ActionResult<IEnumerable<EmployeeV2>> GetAll()
        {
            IEnumerable<EmployeeV2> employeeList = EmployeeV2.GetAll();
            return Ok(employeeList);
        }

        // api/v2/employees/1
        [HttpGet("{id}")]
        public ActionResult<EmployeeV2> Get(int id)
        {
            EmployeeV2 employee = EmployeeV2.GetById(id);

            //if (employee == null || employee.EmployeeCode == "" || employee.EmployeeCode == null)
            if (employee == null || string.IsNullOrEmpty(employee.EmployeeCode))
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // api/employees
        [HttpPost()]
        public ActionResult CreateEmployee(EmployeeV2 employee)
        {
            if (EmployeeV2.ExistsWithEmail(employee.Email))
            {
                ModelState.AddModelError("Email", "Employee already exists with email.");
            }
            if (employee.FirstName == employee.LastName)
            {
                ModelState.AddModelError("FirstName", "FirstName and LastName should be different.");
            }

            if(ModelState.IsValid)
            {
                EmployeeV2.Create(employee);
                return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
            }

            var validation = new ValidationProblemDetails(ModelState);
            validation.Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
            return BadRequest(validation);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateEmployee(int id, EmployeeV2 employee)
        {
            if (EmployeeV2.Exists(id))
            {
                EmployeeV2.Update(id, employee);
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            if (EmployeeV2.Exists(id))
            {
                EmployeeV2.Delete(id);
                return NoContent();
            }
            return NotFound();
        }
    }
}
