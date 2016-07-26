using Microsoft.Practices.Unity;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Logging;

namespace Weather.DependencyResolver
{
    public class Resolver
    {
        public static UnityContainer Container { get; set; }

        public static UnityContainer Bootstrap()
        {
            Container = new UnityContainer();

            Container.RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILog, Log>(new ContainerControlledLifetimeManager());
            return Container;
        }
    }
}