using System.Collections.Generic;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class SensorTypeCore : ISensorTypeCore
    {
        private readonly ISensorTypeRepository _repository;

        public SensorTypeCore(ISensorTypeRepository repository)
        {
            _repository = repository;
        }

        public List<ISensorType> GetAll()
        {
            return _repository.GetAll();
        }

        public ISensorType GetById(int id)
        {
            return _repository.GetById(id);
        }

        public ISensorType Add(ISensorType sensorType)
        {
            sensorType.SensorTypeId = _repository.Add(sensorType);
            return sensorType;
        }

        public void Delete(ISensorType sensorType)
        {
            _repository.Delete(sensorType.SensorTypeId);
        }

        public void Update(ISensorType sensorType)
        {
            _repository.Update(sensorType);
        }

        public ISensorType AddOrUpdate(ISensorType sensorType)
        {
            if (sensorType.SensorTypeId == 0)
            {
                sensorType.SensorTypeId = _repository.Add(sensorType);
                return sensorType;
            }
            Update(sensorType);
            return sensorType;
        }
    }
}