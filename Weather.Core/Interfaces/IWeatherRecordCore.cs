using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface IWeatherRecordCore
    {
        List<IWeatherRecord> GetAllRecords();

        Task<List<IWeatherRecord>> GetAllRecordsForStationBetweenDates(int weatherStationId, DateTime startDate,
            DateTime endDate);

        int Add(IWeatherRecord record);
        int AddWeatherRecordSensorValue(int weatherRecordId, int sensorValueId);
    }
}