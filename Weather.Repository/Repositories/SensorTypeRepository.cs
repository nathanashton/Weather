using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
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
            var sensorTypes = new List<ISensorType>();
            var sql = @"SELECT * FROM SensorTypes";
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
                                    var sensorType = new SensorType
                                    {
                                        SensorTypeId = Convert.ToInt32(reader["SensorTypeId"]),
                                        Name = reader["Name"].ToString()
                                    };
                                    sensorTypes.Add(sensorType);
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
            return sensorTypes;
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
    }
}