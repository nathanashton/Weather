using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class SensorValueRepository : ISensorValueRepository
    {
        private readonly ILog _log;
        private readonly ISensorRepository _sensorRepository;
        private readonly ISettings _settings;

        public SensorValueRepository(ILog log, ISensorRepository sensorRepository, ISettings settings)
        {
            _log = log;
            _sensorRepository = sensorRepository;
            _settings = settings;
        }


        public List<ISensorValue> GetAll()
        {
            _log.Debug("SensorValueRepository.GetAll();");

            var sensorValues = new List<ISensorValue>();
            var sql = @"SELECT * FROM SensorValues";

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
                                    var sensorValue = new SensorValue
                                    {
                                        SensorValueId = Convert.ToInt32(reader["SensorValueId"]),
                                        RawValue = DbUtils.ParseDoubleNull(reader["RawValue"].ToString()),
                                        SensorId = Convert.ToInt32(reader["SensorId"].ToString())
                                    };
                                    sensorValues.Add(sensorValue);
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
            return sensorValues;
        }

        public List<ISensorValue> GetAllTest()
        {
            throw new NotImplementedException();
        }


        public ISensorValue GetById(int id)
        {
            _log.Debug("SensorValueRepository.GetById();");

            SensorValue sensorValue = null;
            var sql = @"SELECT * FROM SensorValues WHERE SensorValueId = @Id";

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
                                    sensorValue = new SensorValue
                                    {
                                        SensorValueId = Convert.ToInt32(reader["SensorValueId"]),
                                        RawValue = DbUtils.ParseDoubleNull(reader["RawValue"].ToString()),
                                        SensorId = Convert.ToInt32(reader["SensorId"].ToString())
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
                throw;
            }
            return sensorValue;
        }


        public int Add(ISensorValue sensorValue)
        {
            _log.Debug("SensorValueRepository.Add();");

            var sql = @"INSERT INTO SensorValues (RawValue, SensorId) VALUES (@RawValue, @SensorId)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@RawValue", sensorValue.RawValue);
                            command.Parameters.AddWithValue("@SensorId", sensorValue.Sensor.SensorId);
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

        public void Delete(int id)
        {
            _log.Debug("SensorValueRepository.Delete();");

            var sql = @"DELETE FROM SensorValues WHERE SensorValueId = @Id";
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

        public void Update(ISensorValue sensorValue)
        {
            _log.Debug("SensorValueRepository.Update();");

            var sql = @"UPDATE SensorValues SET RawValue = @RawValue, SensorId = @SensorId WHERE SensorValueId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@RawValue", sensorValue.RawValue);
                            command.Parameters.AddWithValue("@SensorId", sensorValue.Sensor.SensorId);
                            command.Parameters.AddWithValue("@Id", sensorValue.SensorValueId);

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