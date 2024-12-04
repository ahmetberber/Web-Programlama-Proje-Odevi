using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Data;
using HairSalonManagement.Models;

namespace HairSalonManagement.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = _context.Appointments.Include(a => a.Employee);
            return View(await appointments.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                // Randevu alınacak çalışanların uygunluğunu kontrol et
                var startTime = appointment.Date.TimeOfDay;
                var endTime = startTime.Add(TimeSpan.FromMinutes(appointment.Duration));

                var availableEmployees = _context.Employees
                    .Where(e => e.StartTime <= startTime && e.EndTime >= endTime) // Çalışma saatleri içinde
                    .Where(e => !_context.Appointments
                        .Where(a => a.Date.Date == appointment.Date.Date) // Aynı gün
                        .Where(a => a.EmployeeId == e.Id) // Aynı çalışan
                        .Any(a =>
                            a.Date.TimeOfDay < endTime && // Çakışma kontrolü
                            a.Date.TimeOfDay.Add(TimeSpan.FromMinutes(a.Duration)) > startTime
                        ))
                    .ToList();

                ViewData["EmployeeId"] = new SelectList(availableEmployees, "Id", "Name");
                return View(appointment);
            }

            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", appointment.EmployeeId);
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EmployeeList = new SelectList(_context.Employees, "Id", "Name");
                return View(appointment);
            }

            _context.Update(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }
    }
}
