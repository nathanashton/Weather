using System;
using System.Collections.Generic;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class WeatherRecordCore : IWeatherRecordCore
    {
        private readonly IWeatherRecordRepository _repository;


        public WeatherRecordCore(IWeatherRecordRepository repository)
        {
            _repository = repository;
        }

        public List<IWeatherRecord> GetAllRecords()
        {
            return _repository.GetAll();
        }

        public List<IWeatherRecord> GetAllRecordsForStationBetweenDates(int weatherStationId, DateTime startDate, DateTime endDate)
        {
            return _repository.GetAllForStation(weatherStationId, startDate, endDate);
        }
    }
}