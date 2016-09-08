using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Weather.Common.Entities;
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
        private IUnitRepository _u;
        private ISensorTypeRepository _st;
        private IWeatherStationRepository _ws;


        public WeatherRecordCore(IWeatherRecordRepository repository, ISensorValueRepository sv, ISensorRepository s, IUnitRepository u, ISensorTypeRepository st, IWeatherStationRepository ws)
        {
            _repository = repository;
            _sv = sv;
            _s = s;
            _u = u;
            _st = st;
            _ws = ws;
        }

        public List<IWeatherRecord> GetAllRecords()
        {
            return _repository.GetAll();
        }

        public async Task<List<IWeatherRecord>> GetAllRecordsForStationBetweenDates(int weatherStationId, DateTime startDate, DateTime endDate, Action callback)
        {
            var all = await _repository.GetAllForStation(weatherStationId, startDate, endDate, callback);
            return all;
        }
    }
}