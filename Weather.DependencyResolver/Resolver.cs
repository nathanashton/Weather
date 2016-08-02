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
        private static IUnityContainer _container = null;

        public static IUnityContainer Bootstrap()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                _container.RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ILog, Log>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IStationRepository, StationRepository>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ISensorRepository, SensorRepository>(new ContainerControlledLifetimeManager());

                _container.RegisterType<IStationCore, StationCore>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ISensorCore, SensorCore>(new ContainerControlledLifetimeManager());

            }


            return _container;
        }
    }
}