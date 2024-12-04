using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Data;
using HairSalonManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HairSalonManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var employees = await _context.Employees
                .Include(e => e.EmployeeServices)
                .ThenInclude(es => es.Service).ToListAsync();
            return View(employees);
        }

        // Yeni çalışan oluşturma formu
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // Yeni çalışan kaydet
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee, int[] selectedServices)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
                return View(employee);
            }

            // employee.Services = _context.Services.Where(s => selectedServices.Contains(s.Id)).ToList();
            employee.EmployeeServices = selectedServices.Select(s => new EmployeeService { EmployeeId = employee.Id, ServiceId = s }).ToList();

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Düzenleme formunu getir
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.EmployeeServices)
                .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View(employee);
        }

        // Düzenleme kaydet
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee, int[] selectedServices)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
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
            existingEmployee.EmployeeServices = selectedServices.Select(s => new EmployeeService { EmployeeId = employee.Id, ServiceId = s }).ToList();

            _context.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Silme işlemi öncesi onay sayfasını getir
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

        // Silme işlemini gerçekleştir
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Detayları görüntüle
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.EmployeeServices)
                .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            return View(employee);
        }
    }
}
