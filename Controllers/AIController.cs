using Microsoft.AspNetCore.Mvc;
using HairSalonManagement.Services;
using Microsoft.AspNetCore.Authorization;

namespace HairSalonManagement.Controllers
{
    [Authorize]
    public class AiController : Controller
    {
        private readonly AiService _aiService;

        public AiController(AiService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet]
        public IActionResult Suggestion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Suggestion(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                ViewBag.Error = "Lütfen geçerli bir fotoğraf yükleyin.";
                return View();
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await photo.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                var suggestion = await _aiService.AnalyzeImageAsync(imageBytes);

                ViewBag.Suggestion = suggestion!;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Bir hata oluştu: {ex.Message}";
                return View();
            }
        }
    }
}
