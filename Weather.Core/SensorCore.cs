using System.Collections.Generic;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class SensorCore : ISensorCore
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorCore(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public List<ISensor> GetAllSensors()
        {
            return _sensorRepository.GetAllSensors();
        }

        public ISensor AddOrUpdate(ISensor sensor)
        {
            if (sensor.SensorId == 0)
            {
                sensor.SensorId = _sensorRepository.Add(sensor);
                return sensor;
            }
            Update(sensor);
            return sensor;
        }

        public void Update(ISensor sensor)
        {
            _sensorRepository.Update(sensor);
        }

        public void Delete(ISensor sensor)
        {
            _sensorRepository.Delete(sensor.SensorId);
        }
    }
}