using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface IStationCore
    {
        List<IWeatherStation> GetAllStations();

        IWeatherStation AddOrUpdate(IWeatherStation station);
        void Update(IWeatherStation station);
        void Delete(IWeatherStation station);
        IWeatherStation Add(IWeatherStation station);
        IWeatherStation AddSensorToStation(IStationSensor sensor,IWeatherStation station);
        void RemoveSensorFromStation(IStationSensor sensor, IWeatherStation station);
        bool AnyStationUsesSensor(ISensor sensor);

    }
}