using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface ISensorCore
    {

        List<ISensor> GetAllSensors();
        ISensor AddOrUpdate(ISensor sensor);
        void Update(ISensor sensor);
        void Delete(ISensor sensor);
        bool AnySensorUsesSensorType(ISensorType sensorType);
    }
}