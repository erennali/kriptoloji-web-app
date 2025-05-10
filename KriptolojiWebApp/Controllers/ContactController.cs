using Microsoft.AspNetCore.Mvc;

namespace KriptolojiWebApp.Controllers;

public class ContactController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public ContactController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
}