using Microsoft.Practices.Unity;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core;
using Weather.Core.Interfaces;
using Weather.Logging;
using Weather.Repository.Interfaces;
using Weather.Repository.Repositories;

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
            Container.RegisterType<IStationRepository, StationRepository>();
            Container.RegisterType<ISensorRepository, SensorRepository>();

            Container.RegisterType<IStationCore, StationCore>();
            Container.RegisterType<ISensorCore, SensorCore>();



            return Container;
        }
    }
}