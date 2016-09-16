using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface ISensorTypeCore
    {
        List<ISensorType> GetAll();

        ISensorType GetById(long id);

        ISensorType Add(ISensorType sensorType);

        void Delete(ISensorType sensorType);

        void Update(ISensorType sensorType);

        ISensorType AddOrUpdate(ISensorType sensorType);
    }
}