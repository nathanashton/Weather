using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class SensorCore : ISensorCore
    {
        public void AddSensorValue(SensorValue sensorValue)
        {
            using (var ctx = new Database())
            {
                ctx.SensorValues.Attach(sensorValue);
                ctx.Entry(sensorValue).State = System.Data.Entity.EntityState.Added;
              //  ctx.SensorValues.Add(sensorValue);
                ctx.SaveChanges();
            }
        }

        public void AddSensorValues(IEnumerable<SensorValue> sensorValues)
        {
            using (var ctx = new Database())
            {
                ctx.SensorValues.AddRange(sensorValues);
                ctx.SaveChanges();
            }
        }
    }
}