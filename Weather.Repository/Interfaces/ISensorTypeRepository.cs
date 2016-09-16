using System.Collections.Generic;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface ISensorTypeRepository
    {
        List<ISensorType> GetAll();


        ISensorType GetById(long id);

        long Add(ISensorType sensorType);

        void Delete(long id);

        void Update(ISensorType sensorType);
    }
}