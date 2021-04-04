using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClassicGames.DAL
{
    public static class ClassicGamesDBModule
    {
        public static IServiceCollection LoadDb(this IServiceCollection services, string sqlConnection)
        {
            // EF Core DbContext nesnesi, çalışmak için DbContextOptions'a ihtiyaç duyar.
            var dbOptions = new DbContextOptionsBuilder<CommodoreDBContext>();
            dbOptions.UseSqlServer(sqlConnection); // Options üstünden örneğin bağlantı bilgisini verebiliriz
            services.AddSingleton(o => dbOptions.Options);
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
