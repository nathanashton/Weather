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
    public class SensorTypeRepository : ISensorTypeRepository
    {
        private const string DbConnectionString = @"Data Source=weather.sqlite;Version=3;foreign keys=true;";
        private readonly ILog _log;

        public SensorTypeRepository(ILog log)
        {
            _log = log;
        }

        public List<ISensorType> GetAll()
        {
            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                SensorTypeId = 0,
                Name = String.Empty,
                UnitId = (int?)null,
                DisplayName = String.Empty
            }).ToList();

            var sql = @"SELECT
                        st.[SensorTypeId] as SensorTypeId,
                        st.[Name] as Name,
                        u.[UnitId] as UnitId,
                        u.[DisplayName] as DisplayName
                        FROM [SensorTypes] st
                        LEFT JOIN SensorTypes_Units stu ON st.SensorTypeId = stu.SensorTypeId
                        LEFT JOIN Units u ON stu.UnitId = u.UnitId";
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
                                        SensorTypeId = Convert.ToInt32(reader["SensorTypeId"]),
                                        Name = reader["Name"].ToString(),
                                        UnitId = DbUtils.ParseIntNull(reader["UnitId"].ToString()),
                                        DisplayName = reader["DisplayName"].ToString()
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
            }

            var units = mappedReader.Where(x => x.UnitId != null)
                .GroupBy(x => new { x.UnitId, x.SensorTypeId, x.DisplayName }, x => x, (key, g) =>
                    new
                    {
                        key.SensorTypeId,
                        Unit =
                                               new Unit
                                               {
                                                   UnitId = (int)key.UnitId,
                                                   DisplayName = key.DisplayName
                                               }
                    }).ToList();

            var sensorTypes = mappedReader
                   .GroupBy(x => new { x.SensorTypeId, x.Name }, x => x,
                       (key, g) =>
                           new SensorType
                           {
                               SensorTypeId = key.SensorTypeId,
                               Name = key.Name,
                               Units = units.Where(x => x.SensorTypeId == key.SensorTypeId).Select(x => x.Unit).ToList()
                           }).ToList();

            return sensorTypes.Cast<ISensorType>().ToList();
        }

        public ISensorType GetById(int id)
        {
            ISensorType sensorType = null;
            var sql = @"SELECT * FROM SensorTypes WHERE SensorId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", id);
                            using (var reader = command.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    return null;
                                }
                                while (reader.Read())
                                {
                                    sensorType = new SensorType
                                    {
                                        SensorTypeId = Convert.ToInt32(reader["SensorTypeId"]),
                                        Name = reader["Name"].ToString()
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _log.Error("", ex);
            }
            return sensorType;
        }

        public int Add(ISensorType sensorType)
        {
            var sql = @"INSERT INTO SensorTypes (Name) VALUES (@Name)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Name", sensorType.Name);
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
                return 0;
            }
        }

        public void Delete(int id)
        {
            var sql = @"DELETE FROM SensorTypes WHERE SensorTypeId = @Id";
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
            }
        }

        public void Update(ISensorType sensorType)
        {
            var sql = @"UPDATE SensorTypes SET Name = @Name WHERE SensorTypeId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", sensorType.SensorTypeId);
                            command.Parameters.AddWithValue("@Name", sensorType.Name);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _log.Error("", ex);
            }
        }

        public void AddUnitToSensorType(Unit unit, ISensorType sensorType)
        {
            var sql = @"INSERT INTO SensorTypes_Units (SensorTypeId, UnitId) VALUES (@SensorTypeId, @UnitId)";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@SensorTypeId", sensorType.SensorTypeId);
                            command.Parameters.AddWithValue("@UnitId", unit.UnitId);

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _log.Error("", ex);
            }
        }

        public void RemoveUnitFromSensorType(Unit unit, ISensorType sensortype)
        {
            var sql = @"DELETE FROM SensorTypes_Units WHERE SensorTypeId = @Id AND UnitId = @UnitId";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", sensortype.SensorTypeId);
                            command.Parameters.AddWithValue("@UnitId", unit.UnitId);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _log.Error("", ex);
            }
        }
    }
}