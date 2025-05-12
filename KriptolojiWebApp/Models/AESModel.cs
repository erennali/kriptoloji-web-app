using System.ComponentModel.DataAnnotations;

namespace KriptolojiWebApp.Models
{
    public class AESModel
    {
        [Required(ErrorMessage = "Metin boş bırakılamaz")]
        public string Metin { get; set; }
        
        [Required(ErrorMessage = "Anahtar boş bırakılamaz")]
        public string Anahtar { get; set; }
        public string SifreliMetin { get; set; }
        public string CozulmusMetin { get; set; }
        public string HataMesaji { get; set; }
        public string Mod { get; set; } = "CBC"; 
        public string Padding { get; set; } = "PKCS7"; 
        public int KeySize { get; set; } = 256;
        public string IV { get; set; }  // IV (Initialization Vector)
    }
} 