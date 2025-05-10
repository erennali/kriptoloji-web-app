using Microsoft.AspNetCore.Mvc;

namespace KriptolojiWebApp.Controllers;

public class AESController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public AESController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
}