using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Data;
using HairSalonManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace HairSalonManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int salonId)
        {
            var salon = await _context.Salons
                .Include(e => e.Employees)
                .FirstOrDefaultAsync(s => s.Id == salonId);

            if (salon == null) return NotFound();

            ViewBag.SalonName = salon.Name;
            ViewBag.SalonId = salon.Id;

            if(salon.Employees != null) {
                foreach (var employee in salon.Employees)
                {
                    employee.EmployeeServices = await _context.EmployeeServices
                        .Where(es => es.EmployeeId == employee.Id)
                        .Include(es => es.Service)
                        .ToListAsync();
                }
            }

            return View(salon.Employees);
        }

        [HttpGet]
        public IActionResult Create(int salonId)
        {
            ViewBag.Services = _context.Services.Where(s => s.SalonId == salonId).ToList();
            ViewBag.SalonId = salonId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(Employee employee, int[] EmployeeServices)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = _context.Services.Where(s => s.SalonId == employee.SalonId).ToList();
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return View(employee);
            }

            employee.EmployeeServices = EmployeeServices.Select(s => new EmployeeService { EmployeeId = employee.Id, ServiceId = s }).ToList();
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { salonId = employee.SalonId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.EmployeeServices)
                .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            ViewBag.Services = _context.Services.Where(s => s.SalonId == employee.SalonId).ToList();
            ViewBag.SalonId = employee.SalonId;
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Employee employee, int[] EmployeeServices)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = _context.Services.Where(s => s.SalonId == employee.SalonId).ToList();
                return View(employee);
            }

            var existingEmployee = await _context.Employees
                .Include(e => e.EmployeeServices)
                .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (existingEmployee == null) return NotFound();

            existingEmployee.Name = employee.Name;
            existingEmployee.StartTime = employee.StartTime;
            existingEmployee.EndTime = employee.EndTime;

            existingEmployee.EmployeeServices.Clear();
            existingEmployee.EmployeeServices = EmployeeServices.Select(s => new EmployeeService { EmployeeId = employee.Id, ServiceId = s }).ToList();

            _context.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { salonId = employee.SalonId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.EmployeeServices)
                .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                var SalonId = employee.SalonId;
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { salonId = SalonId });
            }

            return NotFound();
        }
    }
}
