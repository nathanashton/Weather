using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface ISensorRepository
    {
        List<ISensor> GetAllSensors();

        List<ISensor> GetAllSensorsTest();


        long Add(ISensor sensor);

        void Update(ISensor sensor);

        void Delete(long id);

        ISensor GetById(long id);

        bool AnySensorUsesSensorType(ISensorType sensorType);
    }
}