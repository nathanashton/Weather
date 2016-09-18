using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class WeatherRecordCore : IWeatherRecordCore
    {
        private readonly IWeatherRecordRepository _repository;
        private readonly ISensorValueRepository _sv;

        private ISensorRepository _s;
        private ISensorTypeRepository _st;
        private IWeatherStationRepository _ws;


        public WeatherRecordCore(IWeatherRecordRepository repository, ISensorValueRepository sv, ISensorRepository s,
            ISensorTypeRepository st, IWeatherStationRepository ws)
        {
            _repository = repository;
            _sv = sv;
            _s = s;
            _st = st;
            _ws = ws;
        }

        public List<IWeatherRecord> GetAllRecords()
        {
            return _repository.GetAll();
        }

        public async Task<ObservableCollection<IWeatherRecord>> GetAllRecordsForStationBetweenDates(long weatherStationId,
            DateTime startDate, DateTime endDate)
        {
            var all = await _repository.GetAllForStation(weatherStationId, startDate, endDate);
            return all;
        }

        public long Add(IWeatherRecord record)
        {
            return _repository.Add(record);
        }

        public long AddWeatherRecordSensorValue(long weatherRecordId, long sensorValueId)
        {
            return _repository.AddWeatherRecordSensorValue(weatherRecordId, sensorValueId);
        }

        public IWeatherRecord AddRecordAndSensorValues(IWeatherRecord weatherrecord)
        {
            weatherrecord = _repository.AddRecordAndSensorValues(weatherrecord);
            return weatherrecord;
        }

        public List<IWeatherRecord> AddRecordsAndSensorValues(List<IWeatherRecord> weatherrecords)
        {
           weatherrecords = _repository.AddRecordsAndSensorValues(weatherrecords);
            return weatherrecords;
        }
    }
}