using System;
using System.Threading.Tasks;
using KriptolojiWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace KriptolojiWebApp.Services
{
    public interface ISHA256Service
    {
        Task<string> EncryptAsync(string input);
        Task<string> HashFileAsync(IFormFile file);
        Task<string> HashFileFromPathAsync(string filePath);
        Task<(string result, double seconds)> DecryptAsync(string hash, BruteForceParameters parameters);
    }
} 