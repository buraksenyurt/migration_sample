using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClassicGames.DAL
{
    public static class ClassicGamesDBModule
    {
        public static IServiceCollection Load(this IServiceCollection services, string sqlConnection)
        {
            var dbOptions = new DbContextOptionsBuilder<CommodoreDBContext>();
            dbOptions.UseSqlServer(sqlConnection);
            services.AddSingleton(o => dbOptions.Options);
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }
    }
}
