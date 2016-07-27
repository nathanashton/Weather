using System;
using System.Data.SQLite;
using Weather.Common.Entities;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        readonly DbConnection _dbconnection = new DbConnection();
        private readonly SQLiteConnection _connection;

        public SensorRepository()
        {
            _connection = _dbconnection.Connect();
        }
        
        public void Update(Sensor sensor)
        {
            var sql = "UPDATE sensors SET name=@name, unittype_id=@unittypeid, weatherstation_id=@weatherstationid, correction=@correction WHERE Id = @id";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@name", sensor.Name);
            command.Parameters.AddWithValue("@unittypeid", (int)sensor.Type + 1);
            command.Parameters.AddWithValue("@weatherstationid", sensor.Station.Id);
            command.Parameters.AddWithValue("@correction", sensor.Correction);
            command.Parameters.AddWithValue("@id", sensor.Id);
            command.ExecuteNonQuery();
        }

        public void DeleteSensor(Sensor sensor)
        {
            var sql = "DELETE FROM sensors WHERE Id = @id";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@id", sensor.Id);
            command.ExecuteNonQuery();
        }

        public long AddSensor(Sensor sensor)
        {
            var sql = "INSERT INTO sensors (name,unittype_id, weatherstation_id, correction) VALUES (@name, @unittypeid, @weatherstationid, @correction)";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@name", sensor.Name);
            command.Parameters.AddWithValue("@unittypeid", (int)sensor.Type + 1);
            command.Parameters.AddWithValue("@weatherstationid", sensor.Station.Id);
            command.Parameters.AddWithValue("@correction", sensor.Correction);
            command.ExecuteNonQuery();

            var sql2 = "SELECT last_insert_rowid();";
            var command2 = new SQLiteCommand(sql2, _connection);
            var id = command2.ExecuteScalar();
            return (long)id;
        }
    }
}