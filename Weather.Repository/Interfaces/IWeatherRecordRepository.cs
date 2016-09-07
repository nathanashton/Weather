using System;
using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface IWeatherRecordRepository
    {
        List<IWeatherRecord> GetAll();
        List<IWeatherRecord> GetAllForStation(int weatherStationId, DateTime startDate, DateTime endDate);
    }
}