using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private const string DbConnectionString = @"Data Source=weather.sqlite;Version=3;foreign keys=true;";
        private readonly ILog _log;

        public UnitRepository(ILog log)
        {
            _log = log;
        }

        public List<Unit> GetAll()
        {
            var units = new List<Unit>();
            var sql = @"SELECT * FROM Units";
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
                                    var sensorType = new Unit
                                    {
                                        UnitId = Convert.ToInt32(reader["UnitId"]),
                                        DisplayName = reader["DisplayName"].ToString()
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
            }
            return units;
        }

        public Unit GetById(int id)
        {
            Unit unit = null;
            var sql = @"SELECT * FROM Units WHERE UnitId = @Id";
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
                                    unit = new Unit
                                    {
                                        UnitId = Convert.ToInt32(reader["UnitId"]),
                                        DisplayName = reader["DisplayName"].ToString()
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
            return unit;
        }

        public int Add(Unit unit)
        {
            var sql = @"INSERT INTO Units (DisplayName) VALUES (@DisplayName)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@DisplayName", unit.DisplayName);
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
            var sql = @"DELETE FROM Units WHERE UnitId = @Id";
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

        public void Update(Unit unit)
        {
            var sql = @"UPDATE Units SET DisplayName = @DisplayName WHERE UnitId = @Id";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", unit.UnitId);
                            command.Parameters.AddWithValue("@DisplayName", unit.DisplayName);
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