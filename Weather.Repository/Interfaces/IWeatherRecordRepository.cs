using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Weather.Common.Interfaces;
using Weather.Repository.Repositories;

namespace Weather.Repository.Interfaces
{
    public interface IWeatherRecordRepository
    {
        List<IWeatherRecord> GetAll();
        Task<List<Join>> GetAllJoins();

        Task<ObservableCollection<IWeatherRecord>> GetAllForStation(long weatherStationId, DateTime startDate,
            DateTime endDate);

        long Add(IWeatherRecord record);
        long AddWeatherRecordSensorValue(long weatherRecordId, long sensorValueId);
        IWeatherRecord AddRecordAndSensorValues(IWeatherRecord weatherrecord);
        List<IWeatherRecord> AddRecordsAndSensorValues(List<IWeatherRecord> weatherrecords);
    }
}