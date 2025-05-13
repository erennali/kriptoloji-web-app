using Microsoft.AspNetCore.Mvc;
using KriptolojiWebApp.Services;
using KriptolojiWebApp.Models;

namespace KriptolojiWebApp.Controllers;

public class RSASifreleRequest
{
    public string metin { get; set; }
    public string publicKey { get; set; }
}

public class RSAController : Controller
{
    private readonly ILogger<RSAController> _logger;
    private readonly IRSAService _rsaService;

    public RSAController(ILogger<RSAController> logger, IRSAService rsaService)
    {
        _logger = logger;
        _rsaService = rsaService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AnahtarOlustur()
    {
        var (publicKey, privateKey) = _rsaService.AnahtarOlustur();
        return Json(new { publicKey, privateKey });
    }

    [HttpPost]
    public IActionResult Sifrele([FromBody] RSASifreleRequest request)
    {
        try
        {
            var sifreliMetin = _rsaService.Sifrele(request.metin, request.publicKey);
            return Json(new { success = true, sifreliMetin });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şifreleme hatası");
            return Json(new { success = false, message = "Şifreleme işlemi başarısız oldu." });
        }
    }

    public IActionResult Decrypt()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SifreCoz(string sifreliMetin, string privateKey)
    {
        try
        {
            var metin = _rsaService.SifreCoz(sifreliMetin, privateKey);
            return Json(new { success = true, metin });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şifre çözme hatası");
            return Json(new { success = false, message = "Şifre çözme işlemi başarısız oldu." });
        }
    }
}