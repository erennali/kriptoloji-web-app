using System;
using System.Threading.Tasks;
using KriptolojiWebApp.Models;

namespace KriptolojiWebApp.Services
{
    public interface ISHA256Service
    {
        Task<string> EncryptAsync(string input);
        
        Task<(string result, double seconds)> DecryptAsync(string hash, BruteForceParameters parameters);
    }
} 