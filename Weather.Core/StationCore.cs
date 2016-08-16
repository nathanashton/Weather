using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        public ObservableCollection<IWeatherStation> Stations { get; set; }
        public event EventHandler StationsChanged;
        private readonly IStationRepository _stationRepository;


        public StationCore(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
            Stations = new ObservableCollection<IWeatherStation>();
        }


        public Task<IWeatherStation> UpdateStationAsync(IWeatherStation station)
        {
            throw new NotImplementedException();
        }



        public async void GetAllStationsAsync()
        {
            var allStations = await _stationRepository.GetAllWeatherStationsAsync();
            Stations = new ObservableCollection<IWeatherStation>(allStations);
            StationsChanged?.Invoke(this, null);
        }



        public async Task<IWeatherStation> AddStationAsync(IWeatherStation station)
        {
            station.WeatherStationId = await _stationRepository.AddStationAsync(station);
            GetAllStationsAsync();
            OnStationsChanged();
            return station;
        }





        public void DeleteStationAsync(IWeatherStation station)
        {
            throw new NotImplementedException();
        }

        private void OnStationsChanged()
        {
            StationsChanged?.Invoke(this, null);
        }



        public async void DeleteStation(WeatherStation station)
        {
            await _stationRepository.DeleteStationAsync(station);
            GetAllStationsAsync();
            OnStationsChanged();
        }

        public WeatherStation Update(WeatherStation station)
        {
            //if (station.Id == 0)
            //{
            //    station.Id = _stationRepository.AddStationAsync(station);
            //    return station;
            //}
            //station.Id = _stationRepository.UpdateStationAsync(station);
            //GetAllStations();
            //OnStationsChanged();
            return station;
        }

        public void CreateTables()
        {
            _stationRepository.CreateTables();
        }








        public Task<List<IWeatherRecord>> GetRecordsForStationAsync(IWeatherStation station)
        {
            throw new NotImplementedException();
        }






        public List<WeatherRecord> GetRecordsForStation(WeatherStation station)
        {
           // List<WeatherRecord> records = _stationRepository.GetWeatherRecordsForStation(station);




            //foreach (var record in records)
            //{
            //    record.SensorValues = _stationRepository.GetSensorValuesForRecordId(record.Id);
            //}

            return null;
        }
    }
}