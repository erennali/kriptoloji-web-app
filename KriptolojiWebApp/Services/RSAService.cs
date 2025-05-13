using System.Security.Cryptography;
using System.Text;

namespace KriptolojiWebApp.Services;

public class RSAService : IRSAService
{
    public (string publicKey, string privateKey) AnahtarOlustur()
    {
        using var rsa = RSA.Create(2048);
        return (
            Convert.ToBase64String(rsa.ExportRSAPublicKey()),
            Convert.ToBase64String(rsa.ExportRSAPrivateKey())
        );
    }

    public string Sifrele(string metin, string publicKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        byte[] veri = Encoding.UTF8.GetBytes(metin);
        byte[] sifreliVeri = rsa.Encrypt(veri, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(sifreliVeri);
    }

    public string SifreCoz(string sifreliMetin, string privateKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        byte[] sifreliVeri = Convert.FromBase64String(sifreliMetin);
        byte[] veri = rsa.Decrypt(sifreliVeri, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(veri);
    }
} 