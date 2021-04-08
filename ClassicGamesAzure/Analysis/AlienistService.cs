using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Analysis
{
    public class AlienistService
        : IAlienistService
    {
        private readonly ILogger _logger;
        public AlienistService(ILogger logger)
        {
            _logger = logger;
        }
        // İyimserlik belirten kelimeler
        private static readonly string[] optimisticWords = { "harika", "güzel", "iyi", "muhteşem", "olağanüstü", "keyifli", "sevimli" };
        // kötümserlik belirten kelimeler
        private static readonly string[] pessimisticWords = { "kötü", "iğrenç", "çirkin", "sevimsiz", "berbat" };
        public Report GetReport(string content)
        {
            // Belki HTTP Get ile hiçbir içerik gelmez buraya diye bir kontrol.
            if (String.IsNullOrEmpty(content))
                return Report.Unstable;

            _logger.LogInformation($"[{content}] için analiz yapılacak.");
            // Önce içeriği belli karakterlere göre ayırıp kelime kelime ayrıştırıyoruz
            var words = content.Split(new char[] { ' ', ';', ',', '-', '.' });

            // iyimserlik ve kötümserlik skorları
            int optimismCount = 0, pessimismCount = 0;
            foreach (var word in words) //Her bir kelimeyi al
            {
                if (optimisticWords.Contains(word)) // eğer iyimser kelimelerden birisi ise
                    optimismCount++; // iyimserlik skorunu 1 artır
                if (pessimisticWords.Contains(word)) // eğer kötümser kelimelerden birisi ise
                    pessimismCount++; // kötümserlik skorunu 1 artır
            }

            _logger.LogInformation($"Bulunan iyi huylu kelimelerin sayısı {optimismCount} iken kötü huylu olanların sayısı {pessimismCount}.");

            int resultCount = optimismCount - pessimismCount; // net skoru bulalım
                                                              // Elde edilen skorun 0 dan büyük veya küçük olma hallerine bir bakalım
            Report report = resultCount switch
            {
                // net skor sıfırdan büyükse iyimser bir yorum olduğunu döndürebiliriz
                int n when n > 0 => Report.Optimistic,
                int n when n < 0 => Report.Pessimistic,// net skor sıfırdan küçükse kötümser bir yorumdur
                _ => Report.Unstable,// diğer durum olan net skorun sıfır olma halinde ise kararsızlık söz konusudur
            };

            _logger.LogInformation($"Sonuç {report}");
            return report;
        }
    }
}
