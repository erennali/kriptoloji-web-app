using System.Security.Cryptography;
using System.Text;
using KriptolojiWebApp.Models;
using Microsoft.Extensions.Logging;

namespace KriptolojiWebApp.Services
{
    public class AESService : IAESService
    {
        private readonly ILogger<AESService> _logger;

        public AESService(ILogger<AESService> logger)
        {
            _logger = logger;
        }

        public async Task<AESModel> SifreleAsync(AESModel model)
        {
            try
            {
                if (!IsValidKeySize(model.KeySize))
                {
                    throw new ArgumentException($"Geçersiz anahtar boyutu: {model.KeySize}. Anahtar boyutu 128, 192, 256 olabilir");
                }
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = model.KeySize;
                    aes.Mode = GetCipherMode(model.Mod);
                    aes.Padding = GetPaddingMode(model.Padding);

                    byte[] keyBytes = Encoding.UTF8.GetBytes(model.Anahtar);
                    byte[] paddedKey = new byte[model.KeySize / 8];
                    Array.Copy(keyBytes, paddedKey, Math.Min(keyBytes.Length, paddedKey.Length));
                    aes.Key = paddedKey;

                    if (string.IsNullOrEmpty(model.IV))
                    {
                        aes.GenerateIV();
                        model.IV = Convert.ToBase64String(aes.IV);
                    }
                    else
                    {
                        byte[] ivBytes = Encoding.UTF8.GetBytes(model.IV);
                        byte[] paddedIV = new byte[16]; //default
                        Array.Copy(ivBytes, paddedIV, Math.Min(ivBytes.Length, paddedIV.Length));
                        aes.IV = paddedIV;
                    }

                    using (var encryptor = aes.CreateEncryptor())
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            await swEncrypt.WriteAsync(model.Metin);
                        }

                        model.SifreliMetin = Convert.ToBase64String(msEncrypt.ToArray());
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifreleme sırasında hata!");
                model.HataMesaji = $"Şifreleme hatası: {ex.Message}";
                return model;
            }
        }

        public async Task<AESModel> CozAsync(AESModel model)
        {
            try
            {
                if (!IsValidKeySize(model.KeySize))
                {
                    throw new ArgumentException($"Geçersiz anahtar boyutu: {model.KeySize}. Anahtar boyutu 128, 192, 256 olabilir");
                }

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = model.KeySize;
                    aes.Mode = GetCipherMode(model.Mod);
                    aes.Padding = GetPaddingMode(model.Padding);
                    
                    byte[] keyBytes = Encoding.UTF8.GetBytes(model.Anahtar);
                    byte[] paddedKey = new byte[model.KeySize / 8];
                    Array.Copy(keyBytes, paddedKey, Math.Min(keyBytes.Length, paddedKey.Length));
                    aes.Key = paddedKey;
                    
                    byte[] ivBytes = Encoding.UTF8.GetBytes(model.IV);
                    byte[] paddedIV = new byte[16]; 
                    Array.Copy(ivBytes, paddedIV, Math.Min(ivBytes.Length, paddedIV.Length));
                    aes.IV = paddedIV;

                    // decryption işlemi
                    byte[] encryptedBytes = Convert.FromBase64String(model.SifreliMetin);
                    using (var decryptor = aes.CreateDecryptor())
                    using (var msDecrypt = new MemoryStream(encryptedBytes))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        model.CozulmusMetin = await srDecrypt.ReadToEndAsync();
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Çözme sırasında hata oluştu");
                model.HataMesaji = $"Çözme hatası: {ex.Message}";
                return model;
            }
        }

        public async Task<string> RastgeleIVOlusturAsync()
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.GenerateIV();
                    return Convert.ToBase64String(aes.IV);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IV oluşturma sırasında hata oluştu");
                throw new Exception($"IV oluşturma hatası: {ex.Message}");
            }
        }

        public async Task<string> RastgeleAnahtarOlusturAsync(int keySize)
        {
            try
            {
                if (!IsValidKeySize(keySize))
                {
                    throw new ArgumentException($"Geçersiz anahtar boyutu: {keySize}. Anahtar boyutu 128, 192, 256 olabilir");
                }

                int byteLength = keySize / 8;
                
                byte[] key = new byte[byteLength];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(key);
                }
                
                if (key.Length * 8 != keySize)
                {
                    throw new Exception($"Oluşturulan anahtar boyutu ({key.Length * 8} bit) istenen boyut ({keySize} bit) ile uyuşmuyor.");
                }
                
                int actualBitLength = key.Length * 8;
                if (actualBitLength != keySize)
                {
                    throw new Exception($"Anahtar boyutu uyuşmazlığı: İstenen {keySize} bit, Oluşturulan {actualBitLength} bit");
                }
                
                // Base64
                string base64Key = Convert.ToBase64String(key);

                byte[] decodedKey = Convert.FromBase64String(base64Key);
                if (decodedKey.Length * 8 != keySize)
                {
                    throw new Exception($"Base64 dönüşümü sonrası anahtar boyutu değişti: {decodedKey.Length * 8} bit");
                }

                return base64Key;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Anahtar oluşturma sırasında hata oluştu");
                throw new Exception($"Anahtar oluşturma hatası: {ex.Message}");
            }
        }

        private bool IsValidKeySize(int keySize)
        {
            return keySize == 128 || keySize == 192 || keySize == 256;
        }

        private CipherMode GetCipherMode(string mod)
        {
            return mod.ToUpper() switch
            {
                "CBC" => CipherMode.CBC,
                "ECB" => CipherMode.ECB,
                "CFB" => CipherMode.CFB,
                "OFB" => CipherMode.OFB,
                _ => CipherMode.CBC
            };
        }

        private PaddingMode GetPaddingMode(string padding)
        {
            return padding.ToUpper() switch
            {
                "PKCS7" => PaddingMode.PKCS7,
                "ZEROS" => PaddingMode.Zeros,
                "ANSIX923" => PaddingMode.ANSIX923,
                "ISO10126" => PaddingMode.ISO10126,
                _ => PaddingMode.PKCS7
            };
        }
    }
} 