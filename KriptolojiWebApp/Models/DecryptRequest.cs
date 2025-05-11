using System;

namespace KriptolojiWebApp.Models
{
    public class DecryptRequest
    {
        public string Hash { get; set; }
        public BruteForceParameters Parameters { get; set; }
    }
} 