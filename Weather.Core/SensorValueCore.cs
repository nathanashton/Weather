using System.Collections.Generic;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class SensorValueCore : ISensorValueCore
    {
        private readonly ISensorValueRepository _repository;

        public SensorValueCore(ISensorValueRepository repository)
        {
            _repository = repository;
        }

        public List<ISensorValue> GetAll()
        {
            return _repository.GetAll();
        }

        public ISensorValue GetById(int id)
        {
            return _repository.GetById(id);
        }

        public ISensorValue Add(ISensorValue sensorValue)
        {
            sensorValue.SensorValueId = _repository.Add(sensorValue);
            return sensorValue;
        }

        public void Delete(ISensorValue sensorValue)
        {
            _repository.Delete(sensorValue.SensorValueId);
        }

        public void Update(ISensorValue sensorValue)
        {
            _repository.Update(sensorValue);
        }

        public ISensorValue AddOrUpdate(ISensorValue sensorValue)
        {
            if (sensorValue.SensorValueId == 0)
            {
                sensorValue.SensorValueId = _repository.Add(sensorValue);
                return sensorValue;
            }
            Update(sensorValue);
            return sensorValue;
        }
    }
}