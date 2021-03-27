using Autofac;

namespace ClassicGames.DAL
{
    public class ClassicGamesDBModule
        :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //CommodoreDbContext kendisi olarak kayıt edilir
            builder.RegisterType<CommodoreDBContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            // GameRepository, implemente ettiği interface tiplerinin kullanıldığı yerler için kayıt edilir
            builder.RegisterType<GameRepository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
