using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Common.Entities;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface IStationRepository
    {
        Task<List<IWeatherStation>> GetAllWeatherStationsAsync();
        Task<int> AddStationAsync(IWeatherStation station);
        Task<List<ISensor>> GetSensorsForStationAsync(IWeatherStation station);
        Task<List<IWeatherRecord>> GetWeatherRecordsForStationAsync(IWeatherStation station);
        Task<IWeatherStation> GetStationByIdAsync(int id);
        Task<int> UpdateStationAsync(IWeatherStation station);
        Task DeleteStationAsync(IWeatherStation station);

        void CreateTables();




        List<ISensorValue>GetSensorValuesForRecordId(int id);











       



    }
}