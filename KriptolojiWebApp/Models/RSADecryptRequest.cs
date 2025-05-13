
namespace KriptolojiWebApp.Models
{
    public class RSADecryptRequest
    {
        public string Hash { get; set; }
        public BruteForceParameters Parameters { get; set; }
    }
} 