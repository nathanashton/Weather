using Weather.Common.Entities;
using Weather.Repository.Repositories;

namespace Weather.Core
{
    public class Test
    {
        public void go()
        {
            var testStation = new WeatherStation
            {
                Manufacturer = "test manu",
                Model = "Test model",
                Latitude = 0,
                Longitude = 0
            };

            var id = new StationRepository().AddStation(testStation);

            
            var allstations = new StationRepository().GetAllStations();

        }
    }
}
