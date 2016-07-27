using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;
using Weather.Repository.Repositories;

namespace Weather.Core
{
    public class StationCore : IStationCore
    {
        private readonly IStationRepository _stationRepository;

        public StationCore(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public List<WeatherStation> GetAllStations()
        {
            var allStations = _stationRepository.GetAllStations();
            foreach (var station in allStations)
            {
                station.Sensors = new ObservableCollection<ISensor>(_stationRepository.GetSensorsForStation(station));
            }
            return allStations;
        }

        public WeatherStation AddStation(WeatherStation station)
        {
            station.Id = _stationRepository.AddStation(station);
            return station;
        }

        public void DeleteStation(WeatherStation station)
        {
            _stationRepository.DeleteStation(station);
        }

        public WeatherStation Update(WeatherStation station)
        {
            if (station.Id == 0)
            {
                station.Id =_stationRepository.AddStation(station);
                return station;
            }
            station.Id = _stationRepository.Update(station);
            return station;
        }
    }
}