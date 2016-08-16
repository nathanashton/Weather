using System.Collections.Generic;
using Weather.Common.Entities;
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

        public void UpdateSensorForWeatherStation(Sensor sensor)
        {
            _sensorRepository.Update(sensor);
        }

        public void DeleteSensor(Sensor sensor)
        {
            _sensorRepository.DeleteSensor(sensor);
        }

        public Sensor AddSensor(Sensor sensor)
        {
           // sensor.Id = _sensorRepository.AddSensor(sensor);
            return sensor;
        }

        public void AddSensorValue(ISensorValue value)
        {
          //  value.Id = _sensorRepository.InsertSensorValue(value);
        }

        public void AddSensorValues(IEnumerable<ISensorValue> values)
        {
            _sensorRepository.InsertSensorValues(values);
        }

        public void AddWeatherRecords(IEnumerable<IWeatherRecord> records)
        {
            _sensorRepository.InsertWeatherRecords(records);
        }
    }
}