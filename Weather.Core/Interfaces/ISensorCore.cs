using Weather.Common.Entities;

namespace Weather.Core.Interfaces
{
    public interface ISensorCore
    {
        void UpdateSensorForWeatherStation(Sensor sensor);
        void DeleteSensor(Sensor sensor);
        Sensor AddSensor(Sensor sensor);
    }
}