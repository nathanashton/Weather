using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Weather.Common.Entities;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface IStationCore
    {
        void GetAllStationsAsync();





        Task<IWeatherStation> AddStationAsync(IWeatherStation station);







        void DeleteStationAsync(IWeatherStation station);


        Task<IWeatherStation> UpdateStationAsync(IWeatherStation station);



        ObservableCollection<IWeatherStation> Stations { get; set; }
        event EventHandler StationsChanged;
        void CreateTables();


        Task<List<IWeatherRecord>> GetRecordsForStationAsync(IWeatherStation station);
    }
}