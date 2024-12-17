using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Data;
using HairSalonManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace HairSalonManagement.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = _context.Appointments
            .Include(a => a.Employee)
            .Include(a => a.Service)
            .Include(a => a.Salon);

            if(!User.IsInRole("admin"))
            {
                appointments.Where(a => a.CreatedBy == User!.Identity!.Name);
            }

            return View(await appointments.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["SalonList"] = new SelectList(_context.Salons, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Appointment appointment)
        {
            ViewData["SalonList"] = new SelectList(_context.Salons, "Id", "Name");
            ViewData["ServiceList"] = new SelectList(_context.Services.Where(s => s.SalonId == appointment.SalonId), "Id", "Name");
            ViewData["EmployeeList"] = new SelectList(_context.Employees.Where(e => e.SalonId == appointment.SalonId && e.EmployeeServices.Any(s => s.ServiceId == appointment.ServiceId)), "Id", "Name");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                return View(appointment);
            }

            appointment.Salon = _context.Salons.Find(appointment.SalonId)!;
            appointment.Service = _context.Services.Find(appointment.ServiceId)!;
            appointment.Employee = _context.Employees.Find(appointment.EmployeeId)!;
            var startTime = appointment.Date.TimeOfDay;
            var endTime = startTime.Add(TimeSpan.FromMinutes(appointment.Service.Duration));
            var checkAvailability = _context.Appointments.Where(a => a.Date == appointment.Date)
                .Where(a => a.EmployeeId == appointment.EmployeeId).AsEnumerable()
                .Any(a =>
                    a.Date.TimeOfDay < endTime &&
                    a.Date.TimeOfDay.Add(TimeSpan.FromMinutes(a.Service!.Duration)) > startTime
                );

            if (checkAvailability) {
                ModelState.AddModelError(string.Empty, "Employee is not available at that time.");
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

            ViewBag.EmployeeList = new SelectList(_context.Employees, "Id", "Name");
            ViewBag.SalonList = new SelectList(_context.Salons, "Id", "Name");
            ViewBag.ServiceList = new SelectList(_context.Services, "Id", "Name");
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Appointment appointment)
        {
            ViewData["SalonList"] = new SelectList(_context.Salons, "Id", "Name");
            ViewData["ServiceList"] = new SelectList(_context.Services.Where(s => s.SalonId == appointment.SalonId), "Id", "Name");
            ViewData["EmployeeList"] = new SelectList(_context.Employees.Where(e => e.SalonId == appointment.SalonId && e.EmployeeServices.Any(s => s.ServiceId == appointment.ServiceId)), "Id", "Name");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                return View(appointment);
            }

            appointment.Salon = _context.Salons.Find(appointment.SalonId)!;
            appointment.Service = _context.Services.Find(appointment.ServiceId)!;
            appointment.Employee = _context.Employees.Find(appointment.EmployeeId)!;
            var startTime = appointment.Date.TimeOfDay;
            var endTime = startTime.Add(TimeSpan.FromMinutes(appointment.Service.Duration));
            var checkAvailability = _context.Appointments.Where(a => a.Date == appointment.Date)
                .Where(a => a.EmployeeId == appointment.EmployeeId).AsEnumerable()
                .Any(a =>
                    a.Date.TimeOfDay < endTime &&
                    a.Date.TimeOfDay.Add(TimeSpan.FromMinutes(a.Service!.Duration)) > startTime
                );

            if (checkAvailability) {
                ModelState.AddModelError(string.Empty, "Employee is not available at that time.");
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

            appointment.Employee = await _context.Employees.FindAsync(appointment.EmployeeId);
            appointment.Service = await _context.Services.FindAsync(appointment.ServiceId);
            appointment.Salon = await _context.Salons.FindAsync(appointment.SalonId);
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
        public async Task<IActionResult> TakeSalonServices(int salonId)
        {
            var services = await _context.Services.Where(s => s.SalonId == salonId).ToListAsync();
            return Json(services);
        }

        [HttpGet]
        public async Task<IActionResult> TakeSalonEmployeesWithServices(int salonId, int serviceId)
        {
            var employees = await _context.Employees
                .Where(e => e.SalonId == salonId)
                .Where(e => e.EmployeeServices.Any(s => s.ServiceId == serviceId))
                .ToListAsync();
            return Json(employees);
        }
    }
}
