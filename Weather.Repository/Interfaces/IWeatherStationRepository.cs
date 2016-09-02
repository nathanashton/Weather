using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface IWeatherStationRepository
    {
        List<IWeatherStation> GetAllWeatherStations();

        int Add(IWeatherStation station);

        void AddSensorToStation(IStationSensor sensor, IWeatherStation station);

        void Delete(int id);

        void RemoveSensorFromStation(IStationSensor sensor, IWeatherStation station);

        void Update(IWeatherStation station);

        bool AnyStationUsesSensor(ISensor sensor);
    }
}