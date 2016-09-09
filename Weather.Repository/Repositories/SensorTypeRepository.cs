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
    public class SensorTypeRepository : ISensorTypeRepository
    {
        private readonly ILog _log;
        private readonly ISettings _settings;

        public SensorTypeRepository(ILog log, ISettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public List<ISensorType> GetAll()
        {
            _log.Debug("SensorTypeRepository.GetAll();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                SensorTypeId = 0,
                Name = string.Empty,
                UnitId = (int?) null
            }).ToList();

            var sql = @"SELECT
                        st.[SensorTypeId] as SensorTypeId,
                        st.[Name] as Name,
                        st.[UnitTypeId] as UnitTypeId
                        FROM [SensorTypes] st";

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
                                        SensorTypeId = Convert.ToInt32(reader["SensorTypeId"]),
                                        Name = reader["Name"].ToString(),
                                        UnitId = DbUtils.ParseIntNull(reader["UnitTypeId"].ToString())
                                     
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
                .GroupBy(x => new {x.SensorTypeId, x.Name, x.UnitId}, x => x,
                    (key, g) =>
                        new SensorType
                        {
                            SensorTypeId = key.SensorTypeId,
                            Name = key.Name,
                            UnitType = UnitTypes.GetUnitTypeById((int) key.UnitId)
                        }).ToList();

            return sensorTypes.Cast<ISensorType>().ToList();
        }

       

        //TODO
        public ISensorType GetById(int id)
        {
            _log.Debug("SensorTypeRepository.GetById();");

            throw new NotImplementedException();
            //ISensorType sensorType = null;
            //var sql = @"SELECT * FROM SensorTypes WHERE SensorTypeId = @Id";
            //try
            //{
            //    using (var connection = new SQLiteConnection(DbConnectionString))
            //    {
            //        connection.Open();
            //        {
            //            using (var command = new SQLiteCommand(sql, connection))
            //            {
            //                command.Parameters.AddWithValue("@Id", id);
            //                using (var reader = command.ExecuteReader())
            //                {
            //                    if (!reader.HasRows)
            //                    {
            //                        return null;
            //                    }
            //                    while (reader.Read())
            //                    {
            //                        sensorType = new SensorType
            //                        {
            //                            SensorTypeId = Convert.ToInt32(reader["SensorTypeId"]),
            //                            Name = reader["Name"].ToString()
            //                        };
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (SQLiteException ex)
            //{
            //    _log.Error("", ex);
            //    throw;
            //}
            //return sensorType;
        }

        public int Add(ISensorType sensorType)
        {
            _log.Debug("SensorTypeRepository.Add();");

            var sql = @"INSERT INTO SensorTypes (Name, UnitTypeId) VALUES (@Name, @UnitTypeId)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Name", sensorType.Name);
                            command.Parameters.AddWithValue("@UnitTypeId", sensorType.UnitType.UnitTypeId);

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
            _log.Debug("SensorTypeRepository.Delete();");

            var sql = @"DELETE FROM SensorTypes WHERE SensorTypeId = @Id";
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

        public void Update(ISensorType sensorType)
        {
            _log.Debug("SensorTypeRepository.Update();");

            var sql = @"UPDATE SensorTypes SET Name = @Name, UnitTypeId = @UnitTypeId WHERE SensorTypeId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", sensorType.SensorTypeId);
                            command.Parameters.AddWithValue("@Name", sensorType.Name);
                            command.Parameters.AddWithValue("@UnitTypeId", sensorType.UnitType.UnitTypeId);

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