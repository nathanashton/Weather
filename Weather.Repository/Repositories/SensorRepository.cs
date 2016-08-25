using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private const string DbConnectionString = @"Data Source=..\..\..\Weather.Repository\weather.sqlite;Version=3;foreign keys=true;";
        private readonly ILog _log;

        public SensorRepository(ILog log)
        {
            _log = log;
        }

        public List<ISensor> GetAllSensors()
        {
            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                SensorId = 0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                SensorTypeId = 0,
                SensorTypeSensorTypeId =0,
                Name = string.Empty
            }).ToList();

            var sql = @"SELECT
                        s.[SensorId] as SensorId,
                        s.[Manufacturer] as Manufacturer,
                        s.[Model] as Model,
                        s.[Description] as Description,
                        s.[SensorTypeId] as SensorTypeId,
st.[SensorTypeId] as SensorTypeSensorTypeId,
                        st.[Name] as Name
                        FROM [Sensors] s
                        LEFT JOIN SensorTypes st ON s.SensorTypeId = st.SensorTypeId";

            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    mappedReader.Add(new
                                    {
                                        SensorId = Convert.ToInt32(reader["SensorId"]),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        SensorTypeId = Convert.ToInt32(reader["SensorTypeId"]),
                                        SensorTypeSensorTypeId = Convert.ToInt32(reader["SensorTypeSensorTypeId"]),
                                        Name = reader["Name"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _log.Error("", ex);
                throw;
            }





            var sensorTypes = mappedReader
    .GroupBy(x => new { x.SensorTypeSensorTypeId, x.Name, }, x => x, (key, g) =>
          new
          {
              key.SensorTypeSensorTypeId,
              SensorType =
                  new SensorType
                  {
                      SensorTypeId = (int)key.SensorTypeSensorTypeId,
                      Name = key.Name,
                  }
          }).ToList();


            var sensors = mappedReader
                .GroupBy(x => new { x.SensorId, x.Manufacturer, x.Model, x.Description, x.SensorTypeId }, x => x,
                    (key, g) =>
                        new Sensor
                        {
                            SensorId = key.SensorId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Description = key.Description,
                            SensorType = sensorTypes.First(x=> x.SensorTypeSensorTypeId == key.SensorTypeId).SensorType
                        }).ToList();

            return sensors.Cast<ISensor>().ToList();
        }

        public int Add(ISensor sensor)
        {
            var sql = @"INSERT INTO Sensors (Manufacturer, Model, Description, SensorTypeId) VALUES (@Manufacturer, @Model, @Description, @SensorTypeId)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Manufacturer", sensor.Manufacturer);
                            command.Parameters.AddWithValue("@Model", sensor.Model);
                            command.Parameters.AddWithValue("@Description", sensor.Description);
                            command.Parameters.AddWithValue("@SensorTypeId", sensor.SensorType.SensorTypeId);
                            command.ExecuteNonQuery();

                            var command2 = new SQLiteCommand(sql2, connection);
                            var id = command2.ExecuteScalar();
                            return Convert.ToInt32(id);
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

        public void Update(ISensor sensor)
        {
            var sql = @"UPDATE Sensors SET Manufacturer = @Manufacturer, Model = @Model, Description = @Description, SensorTypeId = @SensorTypeId WHERE SensorId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Manufacturer", sensor.Manufacturer);
                            command.Parameters.AddWithValue("@Model", sensor.Model);
                            command.Parameters.AddWithValue("@Description", sensor.Description);
                            command.Parameters.AddWithValue("@SensorTypeId", sensor.SensorType.SensorTypeId);
                            command.Parameters.AddWithValue("@Id", sensor.SensorId);

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

        public void Delete(int id)
        {
            var sql = @"DELETE FROM Sensors WHERE SensorId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", id);
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