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

     
    
        private readonly IWeatherStationRepository _weatherStationRepository;

        public StationCore(IWeatherStationRepository weatherStationRepository)
        {
            _weatherStationRepository = weatherStationRepository;
 }

        public async Task<IWeatherStation> UpdateStationAsync(IWeatherStation station)
        {
          //  await _weatherStationRepository.UpdateStationAsync(station);
            return station;
        }

        public async Task<List<IWeatherStation>> GetAllStationsAsync()
        {
          //  var allStations = await _weatherStationRepository.GetAllWeatherStationsWithSensorsAndRecordsAsync();
            return null;
        }



        public async Task<IWeatherStation> AddStationAsync(IWeatherStation station)
        {
           // station.WeatherStationId = await _weatherStationRepository.AddStationAsync(station);
            return station;
        }
        
        public void DeleteStationAsync(IWeatherStation station)
        {
           // _weatherStationRepository.DeleteStationAsync(station);
        }

      

   
        public WeatherStation Update(WeatherStation station)
        {
            //if (station.Id == 0)
            //{
            //    station.Id = _weatherStationRepository.AddStationAsync(station);
            //    return station;
            //}
            //station.Id = _weatherStationRepository.UpdateStationAsync(station);
            //GetAllStations();
            //OnStationsChanged();
            return station;
        }

        public void CreateTables()
        {
           // _weatherStationRepository.CreateTables();
        }








        public Task<List<IWeatherRecord>> GetRecordsForStationAsync(IWeatherStation station)
        {
            throw new NotImplementedException();
        }






        public List<WeatherRecord> GetRecordsForStation(WeatherStation station)
        {
           // List<WeatherRecord> records = _weatherStationRepository.GetWeatherRecordsForStation(station);




            //foreach (var record in records)
            //{
            //    record.SensorValues = _weatherStationRepository.GetSensorValuesForRecordId(record.Id);
            //}

            return null;
        }
    }
}