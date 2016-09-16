using System.Collections.Generic;
using System.Linq;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class SensorValueCore : ISensorValueCore
    {
        private readonly ISensorValueRepository _repository;
        private readonly ISensorCore _sensorCore;

        public SensorValueCore(ISensorValueRepository repository, ISensorCore sensorCore)
        {
            _repository = repository;
            _sensorCore = sensorCore;
        }

        public List<ISensorValue> GetAll()
        {
            var allSensors = _sensorCore.GetAllSensors();
            var all = _repository.GetAll();
            foreach (var r in all)
            {
                r.Sensor = allSensors.FirstOrDefault(x => x.SensorId == r.SensorId);
            }

            return all;
        }

        public ISensorValue GetById(long id)
        {
            var allSensors = _sensorCore.GetAllSensors();
            var sensorValue = _repository.GetById(id);
            sensorValue.Sensor = allSensors.FirstOrDefault(x => x.SensorId == sensorValue.Sensor.SensorId);
            return sensorValue;
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