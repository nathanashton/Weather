using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface ISensorValueRepository
    {
        List<ISensorValue> GetAll();

        List<ISensorValue> GetAllTest();

        ISensorValue GetById(int id);

        int Add(ISensorValue sensorValue);

        void Delete(int id);

        void Update(ISensorValue sensorValue);
    }
}