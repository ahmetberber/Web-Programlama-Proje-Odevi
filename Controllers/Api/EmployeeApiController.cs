using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Data;
using HairSalonManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace HairSalonManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            // LINQ query to get all employees
            return await _context.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            // LINQ query to find an employee by id
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) {
                return NotFound();
            }
            return employee;
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                // LINQ query to check if an employee exists
                if (!_context.Employees.Any(e => e.Id == id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            // LINQ query to find an employee by id
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
