using Analysis;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/*
 Assembly çalıştırıldığında Startup sınıfının servis kayıt işlemleri için çağırılmasını garanti ediyoruz. 
 */
[assembly: FunctionsStartup(typeof(Startup))]
namespace Analysis
{
    public class Startup
        :FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Servisimizi kayıt ettik. Senaryoda bir servis var ama daha fazla sayıda servis de olabilirdi
            // DI'ı sevin :D
            builder.Services.AddSingleton<IAlienistService, AlienistService>();
        }
    }
}
