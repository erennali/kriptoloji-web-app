using Microsoft.AspNetCore.Mvc;

namespace KriptolojiWebApp.Controllers;

public class SHA256Controller : Controller
{
    private readonly ILogger<HomeController> _logger;

    public SHA256Controller(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
}