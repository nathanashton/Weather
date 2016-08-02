using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        public int Count { get; set; }


        public void Update(Sensor sensor)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql =
                    "UPDATE sensors SET name=@name, unittype_id=@unittypeid, weatherstation_id=@weatherstationid, correction=@correction WHERE Id = @id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", sensor.Name);
                    command.Parameters.AddWithValue("@unittypeid", (int) sensor.Type + 1);
                    command.Parameters.AddWithValue("@weatherstationid", sensor.Station.Id);
                    command.Parameters.AddWithValue("@correction", sensor.Correction);
                    command.Parameters.AddWithValue("@id", sensor.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSensor(Sensor sensor)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql = "DELETE FROM sensors WHERE Id = @id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", sensor.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public long AddSensor(Sensor sensor)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql =
                    "INSERT INTO sensors (name,unittype_id, weatherstation_id, correction) VALUES (@name, @unittypeid, @weatherstationid, @correction)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", sensor.Name);
                    command.Parameters.AddWithValue("@unittypeid", (int) sensor.Type + 1);
                    command.Parameters.AddWithValue("@weatherstationid", sensor.Station.Id);
                    command.Parameters.AddWithValue("@correction", sensor.Correction);
                    command.ExecuteNonQuery();

                    var sql2 = "SELECT last_insert_rowid();";
                    var command2 = new SQLiteCommand(sql2, connection);
                    var id = command2.ExecuteScalar();
                    return (long) id;
                }
            }
        }

        public long InsertWeatherRecord(DateTime timestamp, long sensorValueId, long stationId)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql =
                    "INSERT INTO WeatherRecords (timestamp, sensorvalue_id, station_id) VALUES (@timestamp, @sensorvalueid, @stationid)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@timestamp", timestamp);
                    command.Parameters.AddWithValue("@sensorvalueid", sensorValueId);
                    command.Parameters.AddWithValue("@stationid", stationId);
                    command.ExecuteNonQuery();

                    var sql2 = "SELECT last_insert_rowid();";
                    var command2 = new SQLiteCommand(sql2, connection);
                    var id2 = command2.ExecuteScalar();
                    return (long) id2;
                }
            }
        }

        public void InsertSensorValues(IEnumerable<ISensorValue> sensorvalues)
        {
            var sql = "INSERT INTO SensorValues (value, sensor_id) VALUES (@value, @sensor_id)";

            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "PRAGMA synchronous=OFF";
                    command.ExecuteNonQuery();
                    command.CommandText = "PRAGMA journal_mode=MEMORY";
                    command.ExecuteNonQuery();
                    command.CommandText = "PRAGMA count_changes=OFF";
                    command.ExecuteNonQuery();
                    command.CommandText = "PRAGMA temp_store=MEMORY";
                    command.ExecuteNonQuery();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var value in sensorvalues)
                        {
                            command.CommandText =
                                $"INSERT INTO SensorValues (value, sensor_id) VALUES ('{value.RawValue}', {value.Sensor.Id})";
                            command.ExecuteNonQuery();

                            var sql2 = "SELECT last_insert_rowid();";
                            var command2 = new SQLiteCommand(sql2, connection);
                            var id2 = command2.ExecuteScalar();
                            value.Id = (long) id2;
                        }
                        transaction.Commit();
                        Count++;
                    }
                }
            }
        }

        public void InsertWeatherRecords(IEnumerable<IWeatherRecord> records)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "PRAGMA synchronous=OFF";
                    command.ExecuteNonQuery();
                    command.CommandText = "PRAGMA journal_mode=MEMORY";
                    command.ExecuteNonQuery();
                    command.CommandText = "PRAGMA count_changes=OFF";
                    command.ExecuteNonQuery();
                    command.CommandText = "PRAGMA temp_store=MEMORY";
                    command.ExecuteNonQuery();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var value in records)
                        {
                            foreach (var t in value.SensorValues)
                            {
                                command.CommandText =
                                    $"INSERT INTO WeatherRecords (timestamp, sensorvalue_id, station_id) VALUES ('{value.TimeStamp}', {t.Id}, {t.Sensor.Station.Id})";
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                        Count++;
                    }
                }
            }
        }

        public long InsertSensorValue(ISensorValue sensorvalue)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql = "INSERT INTO SensorValues (value, sensor_id) VALUES (@value, @sensor_id)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@value", sensorvalue.RawValue);
                    command.Parameters.AddWithValue("@sensor_id", sensorvalue.Sensor.Id);
                    command.ExecuteNonQuery();

                    var sql2 = "SELECT last_insert_rowid();";
                    var command2 = new SQLiteCommand(sql2, connection);
                    var id = command2.ExecuteScalar();
                    return (long) id;
                }
            }
        }
    }
}