using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Analysis
{
    public class AlienistFunction
    {
        private readonly IAlienistService _alienistService;
        // AlienistService'i constructor üstünden enjekte ediyoruz
        public AlienistFunction(IAlienistService alienistService)
        {
            _alienistService = alienistService;
        }
        [FunctionName("AlienistFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Yorum için duygu durum analizi fonksiyonu başlatılıyor.");

            // QueryString'teki content değerini alıyoruz. Bunu fonksiyonu URL'den kolayca test etmek için tutuyoruz.
            string content = req.Query["content"];

            /*
                Birde Body ile gelen içeriği JSON formatında alıyoruz.
                Yorum ifadesi büyük bir veri olabileceğinden body ile gelmesi çok daha doğru.
            */
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            content ??= data?.commentText; // body doluysa oradaki commentText değerini alıyoruz

            /*
                İster QueryString üstünden test için ister asıl hedefimize uygun olarak Body ile gelsin
                elimizde bir content olduğunu varsayalım.
                Bunu servisimize gönderip analiz ettiriyor ve cevabı geriye yolluyoruz.
            */
            var result = _alienistService.GetReport(content);

            return new OkObjectResult(result);
        }
    }
}
