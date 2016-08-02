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
        void CreateTables();
        List<WeatherRecord> GetWeatherRecordsForStation(WeatherStation station);
        List<ISensorValue>GetSensorValuesForRecordId(long id);
    }
}