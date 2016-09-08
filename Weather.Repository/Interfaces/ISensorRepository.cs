using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface ISensorRepository
    {
        List<ISensor> GetAllSensors();

        List<ISensor> GetAllSensorsTest();


        int Add(ISensor sensor);

        void Update(ISensor sensor);

        void Delete(int id);

        ISensor GetById(int id);

        bool AnySensorUsesSensorType(ISensorType sensorType);
    }
}