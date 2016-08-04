using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weather.Common.Entities;

namespace Weather.Core.Interfaces
{
    public interface IStationCore
    {
        //List<WeatherStation> GetAllStations();
        //WeatherStation AddStation(WeatherStation station);
        //void DeleteStation(WeatherStation station);
        //WeatherStation Update(WeatherStation station);
        //ObservableCollection<WeatherStation> Stations { get; set; }
        //event EventHandler StationsChanged;
        //void CreateTables();
        //List<WeatherRecord> GetRecordsForStation(WeatherStation station);



        void AddStation(WeatherStation station);
        void DeleteStation(WeatherStation station);
        ObservableCollection<WeatherStation> Stations { get; set; }
        void GetAllStations();
        void DeleteSensor(Sensor sensor);
        void UpdateSensor(Sensor sensor);
        void UpdateStation(WeatherStation station);
        void AddSensor(Sensor sensor);
        void AddWeatherRecord(WeatherRecord record);
        void AddWeatherRecords(IEnumerable<WeatherRecord> records);
    }
}