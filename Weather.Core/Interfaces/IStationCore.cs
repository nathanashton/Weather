using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weather.Common.Entities;

namespace Weather.Core.Interfaces
{
    public interface IStationCore
    {
        List<WeatherStation> GetAllStations();
        WeatherStation AddStation(WeatherStation station);
        void DeleteStation(WeatherStation station);
        WeatherStation Update(WeatherStation station);
        ObservableCollection<WeatherStation> Stations { get; set; }
        event EventHandler StationsChanged;
        void CreateTables();
        List<WeatherRecord> GetRecordsForStation(WeatherStation station);
    }
}