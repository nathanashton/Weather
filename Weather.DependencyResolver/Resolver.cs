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
        public static ILog Log = new Log();
        public static ISettings Settings = new Settings(Log);
        public static ISelectedStation Test = new SelectedStation();

        public IUnityContainer Bootstrap()
        {
            var container = new UnityContainer();

            //container.RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager());
            container.RegisterInstance(Settings);
            container.RegisterInstance(Log);


            container.RegisterInstance(Test);


            container.RegisterType<IStationCore, StationCore>();
            container.RegisterType<ISensorCore, SensorCore>();
            container.RegisterType<IImporter, Importer>();
            container.RegisterType<IUnitTypesCore, UnitTypesCore>();

            container.RegisterType<IWeatherStationRepository, WeatherStationRepository>();
            container.RegisterType<ISensorRepository, SensorRepository>();

            container.RegisterType<ISensorTypeRepository, SensorTypeRepository>();
            container.RegisterType<ISensorTypeCore, SensorTypeCore>();

            container.RegisterType<IUnitRepository, UnitRepository>();
            container.RegisterType<IUnitCore, UnitCore>();

            container.RegisterType<ISensorValueRepository, SensorValueRepository>();
            container.RegisterType<ISensorValueCore, SensorValueCore>();

            container.RegisterType<IWeatherRecordRepository, WeatherRecordRepository>();
            container.RegisterType<IWeatherRecordCore, WeatherRecordCore>();


            container.RegisterType<IStationSensorRepository, StationSensorRepository>();


            return container;
        }
    }
}