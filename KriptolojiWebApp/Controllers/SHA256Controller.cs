using Microsoft.AspNetCore.Mvc;
using KriptolojiWebApp.Services;
using KriptolojiWebApp.Models;

namespace KriptolojiWebApp.Controllers;

public class SHA256Controller : Controller
{
    private readonly ILogger<SHA256Controller> _logger;
    private readonly ISHA256Service _sha256Service;

    public SHA256Controller(ILogger<SHA256Controller> logger, ISHA256Service sha256Service)
    {
        _logger = logger;
        _sha256Service = sha256Service;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Decrypt()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Encrypt(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Json(new { success = false, message = "Input boş olamaz" });
        }

        try
        {
            var sifrelenmisMetin = await _sha256Service.EncryptAsync(input);
            return Json(new { success = true, result = sifrelenmisMetin });
        }
        catch (Exception hata)
        {
            _logger.LogError(hata, "SHA-256 şifrelemesi sırasında hata");
            return Json(new { success = false, message = "Şifreleme sırasında bir hata oluştu" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> HashFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return Json(new { success = false, message = "Dosya seçilmedi veya boş" });
        }

        try
        {
            var hash = await _sha256Service.HashFileAsync(file);
            return Json(new { success = true, result = hash });
        }
        catch (Exception hata)
        {
            _logger.LogError(hata, "Dosya hashleme sırasında hata");
            return Json(new { success = false, message = "Dosya hashleme sırasında bir hata oluştu" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Decrypt([FromBody] DecryptRequest istek)
    {
        if (string.IsNullOrEmpty(istek?.Hash))
        {
            return Json(new { success = false, message = "Hash boş olamaz" });
        }

        if (istek.Parameters == null)
        {
            return Json(new { success = false, message = "Parametreler boş olamaz" });
        }

        try
        {
            var (result, seconds) = await _sha256Service.DecryptAsync(istek.Hash, istek.Parameters);
            return Json(new { success = true, result = result, seconds = seconds });
        }
        catch (Exception hata)
        {
            _logger.LogError(hata, "SHA-256 şifre çözme sırasında hata");
            return Json(new { success = false, message = "Şifre çözme sırasında bir hata oluştu" });
        }
    }
}

