using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface ISensorTypeRepository
    {
        List<ISensorType> GetAll();
        ISensorType GetById(int id);
        int Add(ISensorType sensorType);
        void Delete(int id);
        void Update(ISensorType sensorType);
    }
}