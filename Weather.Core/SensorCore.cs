using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
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
                ctx.SaveChanges();
            }
        }

        public async void AddSensorValues(IEnumerable<SensorValue> sensorValues)
        {





            using (var ctx = new Database())
            {
          //      ctx.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                ctx.Configuration.AutoDetectChangesEnabled = false;
                ctx.Configuration.ValidateOnSaveEnabled = false;
                ctx.SensorValues.AddRange(sensorValues);
              
               await ctx.SaveChangesAsync();
            }

        


        }
    }
}