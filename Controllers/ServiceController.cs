using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSalonManagement.Data;
using HairSalonManagement.Models;

namespace HairSalonManagement.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Belirli bir salon için hizmetleri listele
        public async Task<IActionResult> Index(int salonId)
        {
            var salon = await _context.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.Id == salonId);

            if (salon == null) return NotFound();

            ViewBag.SalonName = salon.Name;
            ViewBag.SalonId = salon.Id;

            return View(salon.Services);
        }

        // Yeni hizmet oluşturma formu
        [HttpGet]
        public IActionResult Create(int salonId)
        {
            ViewBag.SalonId = salonId;
            return View();
        }

        // Yeni hizmet kaydet
        [HttpPost]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SalonId = service.SalonId;
                return View(service);
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { salonId = service.SalonId });
        }

        // Düzenleme formu
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            return View(service);
        }

        // Düzenlemeyi kaydet
        [HttpPost]
        public async Task<IActionResult> Edit(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View(service);
            }

            _context.Update(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { salonId = service.SalonId });
        }

        // Silme işlemi öncesi onay sayfasını getir
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();

            return View(service);
        }

        // Silme işlemini gerçekleştir
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            } else {
                return NotFound();
            }

            return RedirectToAction(nameof(Index), new { salonId = service.SalonId });
        }
    }
}
