using System;
using System.Linq;

namespace Analysis
{
    public class AlienistService
        : IAlienistService
    {
        // İyimserlik belirten kelimeler
        private static readonly string[] optimisticWords = { "harika", "güzel", "iyi", "muhteşem", "olağanüstü", "keyifli", "sevimli" };
        // kötümserlik belirten kelimeler
        private static readonly string[] pessimisticWords = { "kötü", "iğrenç", "çirkin", "sevimsiz", "berbat" };
        public Report GetReport(string content)
        {
            // Önce içeriği belli karakterlere göre ayırıp kelime kelime ayrıştırıyoruz
            var words = content.Split(new char[] { ' ', ';', ',', '-', '.' });

            // iyimserlik ve kötümserlik skorları
            int optimismCount = 0,pessimismCount = 0;
            foreach (var word in words) //Her bir kelimeyi al
            {
                if (optimisticWords.Contains(word)) // eğer iyimser kelimelerden birisi ise
                    optimismCount++; // iyimserlik skorunu 1 artır
                if (pessimisticWords.Contains(word)) // eğer kötümser kelimelerden birisi ise
                    pessimismCount++; // kötümserlik skorunu 1 artır
            }

            int resultCount = optimismCount - pessimismCount; // net skoru bulalım
                                                              // Elde edilen skorun 0 dan büyük veya küçük olma hallerine bir bakalım
            Report report = resultCount switch
            {
                // net skor sıfırdan büyükse iyimser bir yorum olduğunu döndürebiliriz
                int n when n > 0 => Report.Optimistic,
                int n when n < 0 => Report.Pessimistic,// net skor sıfırdan küçükse kötümser bir yorumdur
                _ => Report.Unstable,// diğer durum olan net skorun sıfır olma halinde ise kararsızlık söz konusudur
            };
            return report;
        }
    }
}
