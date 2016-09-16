using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;
using Weather.Units;

namespace Weather.Repository.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ILog _log;
        private readonly ISettings _settings;


        public SensorRepository(ILog log, ISettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public ISensor GetById(long id)
        {
            _log.Debug("SensorRepository.GetById();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                SensorId = (long)0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                SensorTypeId = (long)0,
                SensorTypeSensorTypeId = (long)0,
                Name = string.Empty,
                UnitTypeId = 0
            }).ToList();

            var sql = @"SELECT
                        s.[SensorId] as SensorId,
                        s.[Manufacturer] as Manufacturer,
                        s.[Model] as Model,
                        s.[Description] as Description,
                        s.[SensorTypeId] as SensorTypeId,
                        st.[SensorTypeId] as SensorTypeSensorTypeId,
                        st.[Name] as Name,
                         st.[UnitTypeId] as UnitTypeId
                         FROM [Sensors] s
                        LEFT JOIN SensorTypes st ON s.SensorTypeId = st.SensorTypeId WHERE s.SensorId = @Id";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", id);

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    mappedReader.Add(new
                                    {
                                        SensorId = Convert.ToInt64(reader["SensorId"]),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        SensorTypeId = Convert.ToInt64(reader["SensorTypeId"]),
                                        SensorTypeSensorTypeId = Convert.ToInt64(reader["SensorTypeSensorTypeId"]),
                                        Name = reader["Name"].ToString(),
                                        UnitTypeId = Convert.ToInt32(reader["UnitTypeId"])
                                        
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
                .GroupBy(x => new {x.SensorTypeSensorTypeId, x.Name, x.UnitTypeId}, x => x, (key, g) =>
                    new
                    {
                        key.SensorTypeSensorTypeId,
                        SensorType =
                        new SensorType
                        {
                            SensorTypeId = key.SensorTypeSensorTypeId,
                            Name = key.Name,
                            UnitType = UnitTypes.GetUnitTypeById(key.UnitTypeId)
                        }
                    }).ToList();

            var sensors = mappedReader
                .GroupBy(x => new {x.SensorId, x.Manufacturer, x.Model, x.Description, x.SensorTypeId}, x => x,
                    (key, g) =>
                        new Sensor
                        {
                            SensorId = key.SensorId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Description = key.Description,
                            SensorType = sensorTypes.First(x => x.SensorTypeSensorTypeId == key.SensorTypeId).SensorType,


                        }).ToList();

            return sensors.Cast<ISensor>().First();
        }


        public List<ISensor> GetAllSensorsTest()
        {
            _log.Debug("SensorRepository.GetAllSensors();");
            var Sensors = new List<ISensor>();

            var sql = @"SELECT * FROM Sensors";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var sensor = new Sensor
                                    {
                                        SensorId = Convert.ToInt64(reader["SensorId"]),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        SensorType = null,
                                        SensorValues = null,
                                        SensorTypeId = Convert.ToInt32(reader["SensorTypeId"])
                                    };
                                    Sensors.Add(sensor);
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


            return Sensors;
        }


        public List<ISensor> GetAllSensors()
        {
            _log.Debug("SensorRepository.GetAllSensors();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                SensorId = (long)0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                SensorTypeId = (long)0,
                SensorTypeSensorTypeId = (long)0,
                Name = string.Empty,
                UnitTypeId = 0
            }).ToList();

            var sql = @"SELECT
                        s.[SensorId] as SensorId,
                        s.[Manufacturer] as Manufacturer,
                        s.[Model] as Model,
                        s.[Description] as Description,
                        s.[SensorTypeId] as SensorTypeId,
                        st.[SensorTypeId] as SensorTypeSensorTypeId,
                        st.[Name] as Name,
                        st.[UnitTypeId] as UnitTypeId
                        FROM [Sensors] s
                        LEFT JOIN SensorTypes st ON s.SensorTypeId = st.SensorTypeId";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
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
                                        SensorId = Convert.ToInt64(reader["SensorId"]),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        SensorTypeId = Convert.ToInt64(reader["SensorTypeId"]),
                                        SensorTypeSensorTypeId = Convert.ToInt64(reader["SensorTypeSensorTypeId"]),
                                        Name = reader["Name"].ToString(),
                                        UnitTypeId = Convert.ToInt32(reader["UnitTypeId"])
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
                .GroupBy(x => new {x.SensorTypeSensorTypeId, x.Name, x.UnitTypeId}, x => x, (key, g) =>
                    new
                    {
                        key.SensorTypeSensorTypeId,
                        SensorType =
                        new SensorType
                        {
                            SensorTypeId = key.SensorTypeSensorTypeId,
                            Name = key.Name,
                            UnitType = UnitTypes.GetUnitTypeById(key.UnitTypeId)
                        }
                    }).ToList();

            var sensors = mappedReader
                .GroupBy(x => new {x.SensorId, x.Manufacturer, x.Model, x.Description, x.SensorTypeId}, x => x,
                    (key, g) =>
                        new Sensor
                        {
                            SensorId = key.SensorId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Description = key.Description,
                            SensorType = sensorTypes.First(x => x.SensorTypeSensorTypeId == key.SensorTypeId).SensorType
                        }).ToList();

            return sensors.Cast<ISensor>().ToList();
        }

        public long Add(ISensor sensor)
        {
            _log.Debug("SensorRepository.Add();");

            var sql =
                @"INSERT INTO Sensors (Manufacturer, Model, Description, SensorTypeId) VALUES (@Manufacturer, @Model, @Description, @SensorTypeId)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
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
                            return Convert.ToInt64(id);
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
            _log.Debug("SensorRepository.Update();");

            var sql =
                @"UPDATE Sensors SET Manufacturer = @Manufacturer, Model = @Model, Description = @Description, SensorTypeId = @SensorTypeId WHERE SensorId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
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

        public void Delete(long id)
        {
            _log.Debug("SensorRepository.Delete();");

            var sql = @"DELETE FROM Sensors WHERE SensorId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
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

        public bool AnySensorUsesSensorType(ISensorType sensorType)
        {
            _log.Debug("SensorRepository.AnySensorUsesSensorType();");

            var sql = @"SELECT * FROM Sensors WHERE SensorTypeId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", sensorType.SensorTypeId);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    return true;
                                }
                                return false;
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
        }
    }
}