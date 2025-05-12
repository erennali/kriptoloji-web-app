using System.Security.Cryptography;
using System.Text;
using KriptolojiWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace KriptolojiWebApp.Services
{
    public class SHA256Service : ISHA256Service
    {
        // işlenecek kombinasyon sayısı, değer ne kadar büyükse o kadar çok kombinasyon aynı anda işlenir
        private readonly int _batchSize = 1000;

        /* paralel işlem sayısı ,bilgisayarın işlemci çekirdek sayısı kadar paralel işlem yapılacak
        CPU kullanaraak işlem hızlandırma*/
        private readonly int _maxParallelTasks = Environment.ProcessorCount;

        public async Task<string> EncryptAsync(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                var girilenMetin = Encoding.UTF8.GetBytes(input);
                var hashDegeri = await Task.Run(() => sha256.ComputeHash(girilenMetin));
                
                var sonuc = new StringBuilder();
                foreach (var karakter in hashDegeri)
                {
                    sonuc.Append(karakter.ToString("x2"));
                }
                
                return sonuc.ToString();
            }
        }

        public async Task<string> HashFileAsync(IFormFile dosya)
        {
            if (dosya == null || dosya.Length == 0)
                return string.Empty;

            using (var sha256 = SHA256.Create())
            using (var stream = dosya.OpenReadStream())
            {
                var hashBytes = await sha256.ComputeHashAsync(stream);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString;
            }
        }

        public async Task<string> HashFileFromPathAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(filePath))
            {
                var hashBytes = await sha256.ComputeHashAsync(stream);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString;
            }
        }

        public async Task<(string result, double seconds)> DecryptAsync(string hash, BruteForceParameters parameters)
        {
            if (string.IsNullOrEmpty(hash))
                return (string.Empty, 0);
            
            var startTime = DateTime.Now;
            var karakterler = new List<char>();
            
            if (parameters.IncludeLowercase)
                karakterler.AddRange("abcdefghijklmnopqrstuvwxyz");
            
            if (parameters.IncludeUppercase)
                karakterler.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            
            if (parameters.IncludeNumbers)
                karakterler.AddRange("0123456789");
            
            if (parameters.IncludeSpaces)
                karakterler.Add(' ');
            
            if (parameters.IncludeSpecialChars)
                karakterler.AddRange("!@#$%^&*()_+-=[]{}|;:,.<>?");

            if (!karakterler.Any())
                return ("En az bir karakter seti seçin!", 0);

            if (parameters.MaxLength <= 0)
                return ("Max uzunluk 0'dan büyük olmalı!", 0);

            for (int uzunluk = parameters.MinLength; uzunluk <= parameters.MaxLength; uzunluk++)
            {
                var bulunanSonuc = await TryAllCombinationsParallel(hash, karakterler, uzunluk);
                if (bulunanSonuc != null)
                {
                    var endTime = DateTime.Now;
                    var timeTaken = (endTime - startTime).TotalSeconds;
                    return (bulunanSonuc, timeTaken);
                }
            }
            var finalEndTime = DateTime.Now;
            var finalTimeTaken = (finalEndTime - startTime).TotalSeconds;
            return ("Hash bulunamadı!", finalTimeTaken);
        }

        private async Task<string> TryAllCombinationsParallel(string hedefHash, List<char> karakterler, int uzunluk)
        {
            var toplamKombinasyon = (long)Math.Pow(karakterler.Count, uzunluk);
            //  parçaya bölme işlemi
            var batchCount = (int)Math.Ceiling((double)toplamKombinasyon / _batchSize);

       
            for (int batchIndex = 0; batchIndex < batchCount; batchIndex++)
            {

                var startIndex = batchIndex * _batchSize;
                var endIndex = Math.Min(startIndex + _batchSize, toplamKombinasyon);
                var tasks = new List<Task<string>>();
                var batchSize = (endIndex - startIndex) / _maxParallelTasks;

                for (int i = 0; i < _maxParallelTasks; i++)
                {
                    var taskStartIndex = startIndex + (i * batchSize);
                    var taskEndIndex = i == _maxParallelTasks - 1 ? endIndex : taskStartIndex + batchSize;

                    tasks.Add(ProcessBatch(hedefHash, karakterler, uzunluk, taskStartIndex, taskEndIndex));
                }

                var results = await Task.WhenAll(tasks);
                var foundResult = results.FirstOrDefault(r => r != null);
                if (foundResult != null)
                    return foundResult;
            }

            return null;
        }

        private async Task<string> ProcessBatch(string hedefHash, List<char> karakterler, int uzunluk, long startIndex, long endIndex)
        {
            for (long i = startIndex; i < endIndex; i++)
            {
                var kelime = GenerateWordFromIndex(i, karakterler, uzunluk);
                var hash = await EncryptAsync(kelime);
                if (hash.Equals(hedefHash, StringComparison.OrdinalIgnoreCase))
                    return kelime;
            }
            return null;
        }

        private string GenerateWordFromIndex(long index, List<char> karakterler, int uzunluk)
        {
            // index tabanlı yaklaşım
            var result = new char[uzunluk];
            for (int i = 0; i < uzunluk; i++)
            {
                result[i] = karakterler[(int)(index % karakterler.Count)];
                index /= karakterler.Count;
            }
            return new string(result);
        }
    }
} 