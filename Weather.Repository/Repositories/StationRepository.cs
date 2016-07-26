using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Weather.Common.Entities;

namespace Weather.Repository.Repositories
{
    public class StationRepository
    {
        readonly DbConnection _dbconnection = new DbConnection();
        private readonly SQLiteConnection _connection;

        public StationRepository()
        {
            _connection = _dbconnection.Connect();
        }

        public List<WeatherStation> GetAllStations()
        {
            var stations = new List<WeatherStation>();
            var sql = "SELECT * FROM stations";
            var command = new SQLiteCommand(sql, _connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var newStation = new WeatherStation
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Manufacturer = reader["manufacturer"].ToString(),
                    Model = reader["model"].ToString(),
                    Latitude = Convert.ToDouble(reader["latitude"]),
                    Longitude = Convert.ToDouble(reader["longitude"])
                };
                stations.Add(newStation);
            }
            return stations;
        }

        public long AddStation(WeatherStation station)
        {
            var sql = "INSERT INTO stations (manufacturer, model, latitude, longitude) VALUES (@manufacturer, @model, @latitude, @longitude)";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@manufacturer", station.Manufacturer);
            command.Parameters.AddWithValue("@model", station.Model);
            command.Parameters.AddWithValue("@latitude", station.Latitude);
            command.Parameters.AddWithValue("@longitude", station.Longitude);
            command.ExecuteNonQuery();

            var sql2 = "SELECT last_insert_rowid();";
            var command2 = new SQLiteCommand(sql2, _connection);
            var id = command2.ExecuteScalar();
            return (long)id;
        }
    }
}