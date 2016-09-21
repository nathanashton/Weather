using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface ISensorValueRepository
    {
        List<ISensorValue> GetAll();

        List<ISensorValue> GetAllTest();

        ISensorValue GetById(long id);

        long Add(ISensorValue sensorValue);

        void Delete(long id);
        bool AnyRecordsUseSensor(ISensor sensor);


        void Update(ISensorValue sensorValue);
    }
}