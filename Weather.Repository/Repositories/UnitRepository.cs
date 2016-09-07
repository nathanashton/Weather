using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ILog _log;
        private ISettings _settings;

        public UnitRepository(ILog log, ISettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public List<Unit> GetAll()
        {
            var units = new List<Unit>();
            var sql = @"SELECT * FROM Units";
            _log.Debug("UnitRepository.GetAll();");


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
                                    var sensorType = new Unit
                                    {
                                        UnitId = Convert.ToInt32(reader["UnitId"]),
                                        DisplayName = reader["DisplayName"].ToString(),
                                        DisplayUnit = reader["DisplayUnit"].ToString(),
                                        UnitType =
                                            UnitTypes.UnitsList.FirstOrDefault(
                                                x => x.Name == reader["UnitType"].ToString())
                                    };
                                    units.Add(sensorType);
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
            return units;
        }

        public Unit GetById(int id)
        {
            _log.Debug("UnitRepository.GetById();");

            Unit unit = null;
            var sql = @"SELECT * FROM Units WHERE UnitId = @Id";
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
                                if (!reader.HasRows)
                                {
                                    return null;
                                }
                                while (reader.Read())
                                {
                                    unit = new Unit
                                    {
                                        UnitId = Convert.ToInt32(reader["UnitId"]),
                                        DisplayName = reader["DisplayName"].ToString(),
                                        DisplayUnit = reader["DisplayUnit"].ToString(),
                                        UnitType =
                                            UnitTypes.UnitsList.FirstOrDefault(
                                                x => x.Name == reader["UnitType"].ToString())
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
            return unit;
        }

        public int Add(Unit unit)
        {
            _log.Debug("UnitRepository.Add();");

            var sql =
                @"INSERT INTO Units (DisplayName, DisplayUnit, UnitType) VALUES (@DisplayName, @DisplayUnit, @UnitType)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@DisplayName", unit.DisplayName);
                            command.Parameters.AddWithValue("@DisplayUnit", unit.DisplayUnit);
                            command.Parameters.AddWithValue("@UnitType", unit.UnitType.Name);
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
            _log.Debug("UnitRepository.Delete();");

            var sql = @"DELETE FROM Units WHERE UnitId = @Id";
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

        public void Update(Unit unit)
        {
            _log.Debug("UnitRepository.Update();");

            var sql =
                @"UPDATE Units SET DisplayName = @DisplayName, DisplayUnit = @DisplayUnit, UnitType = @UnitType WHERE UnitId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", unit.UnitId);
                            command.Parameters.AddWithValue("@DisplayName", unit.DisplayName);
                            command.Parameters.AddWithValue("@DisplayUnit", unit.DisplayUnit);
                            command.Parameters.AddWithValue("@UnitType", unit.UnitType.Name);
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