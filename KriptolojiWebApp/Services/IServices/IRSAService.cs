using System.Security.Cryptography;

namespace KriptolojiWebApp.Services;

public interface IRSAService
{
    (string publicKey, string privateKey) AnahtarOlustur();
    string Sifrele(string metin, string publicKey);
    string SifreCoz(string sifreliMetin, string privateKey);
} 