using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Core.Interfaces
{
    public interface IStationCore
    {
        List<WeatherStation> GetAllStations();
        WeatherStation AddStation(WeatherStation station);
        void DeleteStation(WeatherStation station);
        WeatherStation Update(WeatherStation station);
    }
}