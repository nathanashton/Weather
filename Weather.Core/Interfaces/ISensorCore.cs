using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Core.Interfaces
{
    public interface ISensorCore
    {
        //void UpdateSensorForWeatherStation(Sensor sensor);
        //void DeleteSensor(Sensor sensor);
        //Sensor AddSensor(Sensor sensor);
        //void AddSensorValue(ISensorValue value);
        //void AddWeatherRecords(IEnumerable<IWeatherRecord> records);
        //void AddSensorValues(IEnumerable<ISensorValue> values);
        void AddSensorValue(SensorValue sensorValue);
        void AddSensorValues(IEnumerable<SensorValue> sensorValues);
    }
}