using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    [ImplementPropertyChanged]
    public class StationCore : IStationCore
    {
        public ObservableCollection<WeatherStation> Stations { get; set; }
        public event EventHandler StationsChanged;
        private readonly IStationRepository _stationRepository;

        public StationCore(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
            Stations = new ObservableCollection<WeatherStation>();
        }

        public List<WeatherStation> GetAllStations()
        {
            var allStations = _stationRepository.GetAllStations();
            foreach (var station in allStations)
            {
                station.Sensors = new ObservableCollection<ISensor>(_stationRepository.GetSensorsForStation(station));
            }
            Stations = new ObservableCollection<WeatherStation>(allStations);
            return allStations;
        }

        private void OnStationsChanged()
        {
            StationsChanged?.Invoke(this, null);
        }

        public WeatherStation AddStation(WeatherStation station)
        {
            station.Id = _stationRepository.AddStation(station);
            GetAllStations();
            OnStationsChanged();
            return station;
        }

        public void DeleteStation(WeatherStation station)
        {
            _stationRepository.DeleteStation(station);
            GetAllStations();
            OnStationsChanged();
        }

        public WeatherStation Update(WeatherStation station)
        {
            if (station.Id == 0)
            {
                station.Id = _stationRepository.AddStation(station);
                return station;
            }
            station.Id = _stationRepository.Update(station);
            GetAllStations();
            OnStationsChanged();
            return station;
        }

        public void CreateTables()
        {
            _stationRepository.CreateTables();
        }

        public List<WeatherRecord> GetRecordsForStation(WeatherStation station)
        {
            List<WeatherRecord> records = _stationRepository.GetWeatherRecordsForStation(station);




            foreach (var record in records)
            {
                record.SensorValues = _stationRepository.GetSensorValuesForRecordId(record.Id);
            }

            return records;
        }
    }
}