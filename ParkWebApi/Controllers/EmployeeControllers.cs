using Microsoft.AspNetCore.Mvc;
using DataModels;
using System.Linq;
using LinqToDB;

namespace ParkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly P684612TemaDB _context;

        public EmployeesController(P684612TemaDB context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult PostEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("employeeDto is required.");
            }

            var employee = new Employee
            {
                Surname = employeeDto.Surname,
                Name = employeeDto.Name,
                Lastname = employeeDto.Lastname,
                Birthday = employeeDto.Birthday,
                Gender = employeeDto.Gender,
                Phone = employeeDto.Phone,
                Email = employeeDto.Email,
                Adress = employeeDto.Adress,
                PositionId = employeeDto.PositionId,
                AttractionId = employeeDto.AttractionId
            };

            _context.Insert(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }


        [HttpPut("{id}")]
        public IActionResult PutEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            if (id != employeeDto.EmployeeId)
            {
                return BadRequest();
            }

            var employee = new Employee
            {
                EmployeeId = employeeDto.EmployeeId,
                Surname = employeeDto.Surname,
                Name = employeeDto.Name,
                Lastname = employeeDto.Lastname,
                Birthday = employeeDto.Birthday,
                Gender = employeeDto.Gender,
                Phone = employeeDto.Phone,
                Email = employeeDto.Email,
                Adress = employeeDto.Adress,
                PositionId = employeeDto.PositionId,
                AttractionId = employeeDto.AttractionId
            };

            _context.Update(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Delete(employee);
            return NoContent();
        }
    }
}
