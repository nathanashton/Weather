﻿using PropertyChanged;
using System.Collections.Generic;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    [ImplementPropertyChanged]
    public class StationCore : IStationCore
    {
        private readonly IWeatherStationRepository _weatherStationRepository;
        private readonly IStationSensorRepository _stationSensorRepository;

        public StationCore(IWeatherStationRepository weatherStationRepository, IStationSensorRepository stationSensorRepository)
        {
            _weatherStationRepository = weatherStationRepository;
            _stationSensorRepository = stationSensorRepository;
        }

        public List<IWeatherStation> GetAllStations()
        {
            return _weatherStationRepository.GetAllWeatherStations();
        }

        public IWeatherStation AddOrUpdate(IWeatherStation station)
        {
            if (station.WeatherStationId == 0)
            {
                station.WeatherStationId = _weatherStationRepository.Add(station);
                return station;
            }
            Update(station);
            return station;
        }

        public void Update(IWeatherStation station)
        {
            _weatherStationRepository.Update(station);
        }

        public void Delete(IWeatherStation station)
        {
            _weatherStationRepository.Delete(station.WeatherStationId);
        }

        public IWeatherStation Add(IWeatherStation station)
        {
            station.WeatherStationId = _weatherStationRepository.Add(station);
            return station;
        }

        public IWeatherStation AddSensorToStation(IStationSensor sensor, IWeatherStation station)
        {
            _weatherStationRepository.AddSensorToStation(sensor, station);
            return station;
        }

        public void RemoveSensorFromStation(IStationSensor sensor, IWeatherStation station)
        {
            _weatherStationRepository.RemoveSensorFromStation(sensor, station);
        }

        public bool AnyStationUsesSensor(ISensor sensor)
        {
            return _weatherStationRepository.AnyStationUsesSensor(sensor);
        }

        public void UpdateStationSensor(IStationSensor stationSensor)
        {
            _stationSensorRepository.Update(stationSensor);
        }
    }
}