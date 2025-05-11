using System;

namespace KriptolojiWebApp.Models
{
    public class BruteForceParameters
    {
        public bool IncludeLowercase { get; set; }
        public bool IncludeUppercase { get; set; }
        public bool IncludeNumbers { get; set; }
        public bool IncludeSpaces { get; set; }
        public bool IncludeSpecialChars { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
    }
} 