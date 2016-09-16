using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface IWeatherRecordCore
    {
        List<IWeatherRecord> GetAllRecords();

        Task<List<IWeatherRecord>> GetAllRecordsForStationBetweenDates(long weatherStationId, DateTime startDate,
            DateTime endDate);

        long Add(IWeatherRecord record);
        long AddWeatherRecordSensorValue(long weatherRecordId, long sensorValueId);

        IWeatherRecord AddRecordAndSensorValues(IWeatherRecord weatherrecord);
        List<IWeatherRecord> AddRecordsAndSensorValues(List<IWeatherRecord> weatherrecords);

    }
}