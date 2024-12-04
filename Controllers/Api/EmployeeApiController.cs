using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Data;
using HairSalonManagement.Models;

namespace HairSalonManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Tüm çalışanları getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // Çalışanların uygunluk durumunu kontrol et
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAvailableEmployees(DateTime Date, int duration)
        {
            var startTime = Date.TimeOfDay;
            var endTime = startTime.Add(TimeSpan.FromMinutes(duration));

            // Çalışma saatleri ve mevcut randevulara göre uygun çalışanları filtrele
            var availableEmployees = await _context.Employees
                .Where(e => e.StartTime <= startTime && e.EndTime >= endTime) // Çalışma saatleri içinde
                .Where(e => !_context.Appointments
                    .Where(a => a.Date.Date == Date.Date) // Aynı gün
                    .Where(a => a.EmployeeId == e.Id) // Aynı çalışan
                    .Any(a =>
                        a.Date.TimeOfDay < endTime && // Çakışma kontrolü
                        a.Date.TimeOfDay.Add(TimeSpan.FromMinutes(a.Duration)) > startTime
                    ))
                .ToListAsync();

            return availableEmployees;
        }

        // 3. ID ile çalışan getir
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        // 4. Yeni çalışan ekle
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // 5. Çalışanı düzenle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // 6. Çalışanı sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
