using ClassicGames.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ClassicGames.WebClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.LoadDb(Configuration.GetConnectionString("CommodoreDB"));

            /*
            Azure fonksiyonunu kullanmak için gerekli konfigurasyon ayarlarını appSettings.json içerisinden okuyacağız.
            Buradaki json bölümünü AlienistServiceSettings sınıfına bağlamak için aşağıdaki kod parçasını kullanıyoruz.
             */
            var alienistServiceSettings = new AlienistServiceSettings();
            Configuration.GetSection("AlienistServiceSettings").Bind(alienistServiceSettings);
            services.AddSingleton(alienistServiceSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(app =>
                {
                    app.Run(async context =>
                    {
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;
                        if (exception != null)
                        {
                            Log.Error(exception, exception.Message);
                        }
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync("<html><body>Internal Server Error.</body></html>");

                    }
                    );
                }
                );
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
