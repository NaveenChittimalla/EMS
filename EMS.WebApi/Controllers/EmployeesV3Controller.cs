using EMS.CoreLibrary.Models;
using EMS.CoreLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApi.Controllers
{
    [Route("api/v3/employees")]
    [ApiController]
    public class EmployeesV3Controller : ControllerBase
    {
        private readonly EmployeeSqlDbRepository _employeeDbRepository;

        public EmployeesV3Controller()
        {
            _employeeDbRepository = new EmployeeSqlDbRepository();   
        }

        // api/v3/employees -- All Employees
        [HttpGet()]
        public ActionResult<IEnumerable<EmployeeV3>> GetAll()
        {
            IEnumerable<EmployeeV3> employeeList = _employeeDbRepository.GetAll();
            return Ok(employeeList);
        }

        // api/v3/employees/1
        [HttpGet("{id}")]
        public ActionResult<EmployeeV3> Get(int id)
        {
            EmployeeV3 employee = _employeeDbRepository.GetById(id);

            if (employee == null || string.IsNullOrEmpty(employee.EmployeeCode))
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // api/v3/employees
        [HttpPost()]
        public ActionResult CreateEmployee(EmployeeV3 employee)
        {
            if (_employeeDbRepository.ExistsWithEmail(employee.Email))
            {
                ModelState.AddModelError("Email", "Employee already exists with email.");
            }
            if (employee.FirstName == employee.LastName)
            {
                ModelState.AddModelError("FirstName", "FirstName and LastName should be different.");
            }

            if(ModelState.IsValid)
            {
                _employeeDbRepository.Create(employee);
                return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
            }

            var validation = new ValidationProblemDetails(ModelState);
            validation.Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
            return BadRequest(validation);
        }

        // api/v3/employees/1
        [HttpPut("{id}")]
        public ActionResult UpdateEmployee(int id, EmployeeV3 employee)
        {
            if (_employeeDbRepository.Exists(id))
            {
                _employeeDbRepository.Update(id, employee);
                return NoContent();
            }
            return NotFound();
        }

        // api/v3/employees/1
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            if (_employeeDbRepository.Exists(id))
            {
                _employeeDbRepository.Delete(id);
                return NoContent();
            }
            return NotFound();
        }
    }
}
