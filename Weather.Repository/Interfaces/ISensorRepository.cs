using System;
using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Common.Interfaces;

namespace Weather.Repository.Interfaces
{
    public interface ISensorRepository
    {
        void Update(Sensor sensor);
        void DeleteSensor(Sensor sensor);
        long AddSensor(Sensor sensor);

        long InsertWeatherRecord(DateTime timestamp, long sensorValueId, long stationId);


        long InsertSensorValue(ISensorValue sensorvalue);
        void InsertSensorValues(IEnumerable<ISensorValue> sensorvalues);
        void InsertWeatherRecords(IEnumerable<IWeatherRecord> records);
        int Count { get; set; }
    }
}