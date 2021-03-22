using Autofac;

namespace ClassicGames.DAL
{
    public class ClassicGamesDBModule
        :Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommodoreDBContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<GameRepository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
