using Weather.Common.Entities;

namespace Weather.Repository.Interfaces
{
    public interface ISensorRepository
    {
        void Update(Sensor sensor);
        void DeleteSensor(Sensor sensor);
        long AddSensor(Sensor sensor);
    }
}