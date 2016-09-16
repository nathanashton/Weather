using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface IStationSensorRepository
    {
        IStationSensor GetById(long id);

        void Update(IStationSensor stationSensor);
    }
}