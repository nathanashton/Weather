using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class StationSensorRepository : IStationSensorRepository
    {

        private const string DbConnectionString = @"Data Source=..\..\..\Weather.Repository\weather.sqlite;Version=3;foreign keys=true;";
        private ILog _log;

        public StationSensorRepository(ILog log)
        {
            _log = log;
        }

        public IStationSensor GetById(int id)
        {
            return null;
        }

        public void Update(IStationSensor stationSensor)
        {
            var sql = @"UPDATE WeatherStations_Sensors SET SensorId = @SensorId, Correction = @Correction, Notes = @Notes WHERE Id = @StationSensorId";

            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@SensorId", stationSensor.Sensor.SensorId);
                            command.Parameters.AddWithValue("@Correction", stationSensor.Correction);
                            command.Parameters.AddWithValue("@Notes", stationSensor.Notes);
                            command.Parameters.AddWithValue("@StationSensorId", stationSensor.StationSensorId);
                            command.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _log.Error("", ex);
                throw;
            };
        }
    }
}
