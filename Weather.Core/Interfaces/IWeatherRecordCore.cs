using System;
using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface IWeatherRecordCore
    {
        List<IWeatherRecord> GetAllRecords();
        List<IWeatherRecord> GetAllRecordsForStationBetweenDates(int weatherStationId, DateTime startDate, DateTime endDate);
    }
}