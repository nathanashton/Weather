using System;
using System.Collections.Generic;
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

        public async Task<List<IWeatherRecord>> GetAllRecordsForStationBetweenDates(int weatherStationId,
            DateTime startDate, DateTime endDate)
        {
            var all = await _repository.GetAllForStation(weatherStationId, startDate, endDate);
            return all;
        }

        public int Add(IWeatherRecord record)
        {
            return _repository.Add(record);
        }

        public int AddWeatherRecordSensorValue(int weatherRecordId, int sensorValueId)
        {
            return _repository.AddWeatherRecordSensorValue(weatherRecordId, sensorValueId);
        }

        public IWeatherRecord AddRecordAndSensorValues(IWeatherRecord weatherrecord)
        {
            throw new NotImplementedException();
        }
    }
}