using Microsoft.AspNetCore.Mvc;

namespace KriptolojiWebApp.Controllers;

public class RSAController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public RSAController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
}