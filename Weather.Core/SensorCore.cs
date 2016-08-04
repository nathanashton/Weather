using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class SensorCore : ISensorCore
    {
        private readonly Database _context;

        public SensorCore(Database db)
        {
            _context = db;
        }

        public void AddSensorValue(SensorValue sensorValue)
        {
            _context.SensorValues.Add(sensorValue);
            _context.SaveChanges();
        }

        public void AddSensorValues(IEnumerable<SensorValue> sensorValues)
        {
            _context.SensorValues.AddRange(sensorValues);
            _context.SaveChanges();
        }
    }
}