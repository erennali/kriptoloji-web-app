using Microsoft.AspNetCore.Mvc;
using KriptolojiWebApp.Models;
using KriptolojiWebApp.Services;

namespace KriptolojiWebApp.Controllers;

public class AESController : Controller
{
    private readonly ILogger<AESController> _logger;
    private readonly IAESService _aesService;

    public AESController(ILogger<AESController> logger, IAESService aesService)
    {
        _logger = logger;
        _aesService = aesService;
    }

    public IActionResult Index()
    {
        return View(new AESModel());
    }

    public IActionResult Decrypt()
    {
        return View(new AESModel());
    }

    [HttpPost]
    public async Task<IActionResult> Sifrele([FromBody] AESModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Metin) || string.IsNullOrEmpty(model.Anahtar))
            {
                return Json(new { hataMesaji = "Lütfen metin ve anahtar alanlarını doldurun!" });
            }

            var sonuc = await _aesService.SifreleAsync(model);
            return Json(sonuc);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Şifreleme sırasında hata oluştu");
            return Json(new { hataMesaji = $"Şifreleme hatası: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Coz([FromBody] AESModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.SifreliMetin) || string.IsNullOrEmpty(model.Anahtar) || string.IsNullOrEmpty(model.IV))
            {
                return Json(new { hataMesaji = "Lütfen şifreli metin, anahtar ve IV alanlarını doldurun!" });
            }

            var sonuc = await _aesService.CozAsync(model);
            return Json(sonuc);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Çözme sırasında hata oluştu");
            return Json(new { hataMesaji = $"Çözme hatası: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RastgeleIVOlustur()
    {
        try
        {
            var iv = await _aesService.RastgeleIVOlusturAsync();
            return Json(new { iv });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "IV oluşturma sırasında hata oluştu");
            return Json(new { hataMesaji = $"IV oluşturma hatası: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RastgeleAnahtarOlustur(int keySize)
    {
        try
        {
            var anahtar = await _aesService.RastgeleAnahtarOlusturAsync(keySize);
            return Json(new { anahtar });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Anahtar oluşturma sırasında hata oluştu");
            return Json(new { hataMesaji = $"Anahtar oluşturma hatası: {ex.Message}" });
        }
    }
}