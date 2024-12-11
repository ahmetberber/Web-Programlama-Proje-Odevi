using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web_Programlama_Proje_Odevi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Web_Programlama_Proje_Odevi.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
