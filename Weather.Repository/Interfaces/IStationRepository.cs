using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface IStationRepository
    {
        List<WeatherStation> GetAllStations();
        long AddStation(WeatherStation station);
        List<ISensor> GetSensorsForStation(WeatherStation station);
        WeatherStation GetStationById(long id);
        void DeleteStation(WeatherStation station);
        long Update(WeatherStation station);
    }
}