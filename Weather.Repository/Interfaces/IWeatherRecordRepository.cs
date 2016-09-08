using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Common.Interfaces;
using Weather.Repository.Repositories;

namespace Weather.Repository.Interfaces
{
    public interface IWeatherRecordRepository
    {
        List<IWeatherRecord> GetAll();
        Task<List<Join>> GetAllJoins();

        Task<List<IWeatherRecord>> GetAllTestAsync();

        Task<List<IWeatherRecord>> GetAllForStation(int weatherStationId, DateTime startDate, DateTime endDate, Action callback);
    }
}