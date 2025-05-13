using System.Security.Cryptography;
using System.Text;

namespace KriptolojiWebApp.Services;

public class RSAService : IRSAService
{
    public (string publicKey, string privateKey) AnahtarOlustur()
    {
        using var rsa = RSA.Create(2048);
        var publicKey = ExportPublicKeyToPEM(rsa);
        var privateKey = ExportPrivateKeyToPEM(rsa);
        return (publicKey, privateKey);
    }

    public string Sifrele(string metin, string publicKey)
    {
        using var rsa = RSA.Create();
        ImportPublicKeyFromPEM(rsa, publicKey);
        byte[] veri = Encoding.UTF8.GetBytes(metin);
        byte[] sifreliVeri = rsa.Encrypt(veri, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(sifreliVeri);
    }

    public string SifreCoz(string sifreliMetin, string privateKey)
    {
        using var rsa = RSA.Create();
        ImportPrivateKeyFromPEM(rsa, privateKey);
        byte[] sifreliVeri = Convert.FromBase64String(sifreliMetin);
        byte[] veri = rsa.Decrypt(sifreliVeri, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(veri);
    }

    private string ExportPublicKeyToPEM(RSA rsa)
    {
        var pub = rsa.ExportSubjectPublicKeyInfo();
        var base64 = Convert.ToBase64String(pub);
        var sb = new StringBuilder();
        sb.AppendLine("-----BEGIN PUBLIC KEY-----");
        for (int i = 0; i < base64.Length; i += 64)
            sb.AppendLine(base64.Substring(i, Math.Min(64, base64.Length - i)));
        sb.AppendLine("-----END PUBLIC KEY-----");
        return sb.ToString();
    }

    private string ExportPrivateKeyToPEM(RSA rsa)
    {
        var priv = rsa.ExportPkcs8PrivateKey();
        var base64 = Convert.ToBase64String(priv);
        var sb = new StringBuilder();
        sb.AppendLine("-----BEGIN PRIVATE KEY-----");
        for (int i = 0; i < base64.Length; i += 64)
            sb.AppendLine(base64.Substring(i, Math.Min(64, base64.Length - i)));
        sb.AppendLine("-----END PRIVATE KEY-----");
        return sb.ToString();
    }

    private void ImportPublicKeyFromPEM(RSA rsa, string pem)
    {
        var base64 = pem.Replace("-----BEGIN PUBLIC KEY-----", "")
                        .Replace("-----END PUBLIC KEY-----", "")
                        .Replace("\r", "")
                        .Replace("\n", "")
                        .Trim();
        var key = Convert.FromBase64String(base64);
        rsa.ImportSubjectPublicKeyInfo(key, out _);
    }

    private void ImportPrivateKeyFromPEM(RSA rsa, string pem)
    {
        var base64 = pem.Replace("-----BEGIN PRIVATE KEY-----", "")
                        .Replace("-----END PRIVATE KEY-----", "")
                        .Replace("\r", "")
                        .Replace("\n", "")
                        .Trim();
        var key = Convert.FromBase64String(base64);
        rsa.ImportPkcs8PrivateKey(key, out _);
    }
} 