using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface IStationSensorRepository
    {
        IStationSensor GetById(int id);

        void Update(IStationSensor stationSensor);
    }
}