using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Core.Interfaces
{
    public interface ISensorValueCore
    {
        List<ISensorValue> GetAll();

        ISensorValue GetById(int id);

        ISensorValue Add(ISensorValue sensorValue);

        void Delete(ISensorValue sensorValue);

        void Update(ISensorValue sensorValue);

        ISensorValue AddOrUpdate(ISensorValue sensorValue);
    }
}