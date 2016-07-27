using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class StationRepository : IStationRepository
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

        public List<ISensor> GetSensorsForStation(WeatherStation station)
        {
            var sensors = new List<ISensor>();
            var sql = "SELECT * FROM sensors LEFT JOIN unittypes ON sensors.unittype_id = unittypes.id WHERE weatherstation_id = @stationid";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@stationid", station.Id);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var typ = reader[6].ToString();
                var pp = (Enums.UnitType) System.Enum.Parse(typeof(Enums.UnitType), typ);

                var correction = reader["correction"].ToString();

                var sensor = new Sensor
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                    Station = GetStationById(Convert.ToInt32(reader["weatherstation_id"])),
                    Correction = Convert.ToDouble(reader["correction"].ToString()),
                    Type = pp
                };
                sensors.Add(sensor);
            }
            return sensors;
        }

        public WeatherStation GetStationById(long id)
        {
            WeatherStation station = new WeatherStation();
            var sql = "SELECT * FROM stations WHERE Id = @id";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@id", id);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                station = new WeatherStation
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Manufacturer = reader["manufacturer"].ToString(),
                    Model = reader["model"].ToString(),
                    Latitude = Convert.ToDouble(reader["latitude"]),
                    Longitude = Convert.ToDouble(reader["longitude"])
                };
            }
            return station;
        }

        public void DeleteStation(WeatherStation station)
        {
            var sql = "DELETE FROM stations WHERE Id = @id";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@id", station.Id);
            command.ExecuteNonQuery();
        }

        public long Update(WeatherStation station)
        {
            var sql = "UPDATE stations SET manufacturer=@manufacturer, model=@model, longitude=@longitude, latitude=@latitude WHERE Id = @id";
            var command = new SQLiteCommand(sql, _connection);
            command.Parameters.AddWithValue("@manufacturer", station.Manufacturer);
            command.Parameters.AddWithValue("@model", station.Model);
            command.Parameters.AddWithValue("@longitude", station.Longitude);
            command.Parameters.AddWithValue("@latitude", station.Latitude);
            command.Parameters.AddWithValue("@id", station.Id);
            command.ExecuteNonQuery();
            return station.Id;
        }
    }
}