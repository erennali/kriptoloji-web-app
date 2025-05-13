using KriptolojiWebApp.Models;

namespace KriptolojiWebApp.Services
{
    public interface IAESService
    {
        Task<AESModel> SifreleAsync(AESModel model);
        Task<AESModel> CozAsync(AESModel model);
        Task<string> RastgeleIVOlusturAsync();
        Task<string> RastgeleAnahtarOlusturAsync(int keySize);
    }
} 