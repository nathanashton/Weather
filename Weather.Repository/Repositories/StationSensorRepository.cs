using System.Data.SQLite;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class StationSensorRepository : IStationSensorRepository
    {
        private readonly ILog _log;
        private readonly ISettings _settings;

        public StationSensorRepository(ILog log, ISettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public IStationSensor GetById(int id)
        {
            return null;
        }

        public void Update(IStationSensor stationSensor)
        {
            _log.Debug("StationSensorRepository.Update();");

            var sql =
                @"UPDATE WeatherStations_Sensors SET SensorId = @SensorId, Correction = @Correction, Notes = @Notes WHERE Id = @StationSensorId";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
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
            }
        }
    }
}