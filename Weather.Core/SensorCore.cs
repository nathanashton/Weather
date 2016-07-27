using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Weather.Common.Entities;
using Weather.Core.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Core
{
    public class SensorCore : ISensorCore
    {

        private ISensorRepository _sensorRepository;

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
            sensor.Id = _sensorRepository.AddSensor(sensor);
            return sensor;
        }

    }
}
