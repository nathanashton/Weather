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
        public List<WeatherStation> GetAllStations()
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();
                var stations = new List<WeatherStation>();
                var sql = "SELECT * FROM stations";
                using (var command = new SQLiteCommand(sql, connection))
                {
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
            }
        }









        public List<WeatherRecord> GetWeatherRecordsForStation(WeatherStation station)
        {
            var records = new List<WeatherRecord>();
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString =
                        "Data Source=weather.sqlite;Version=3;foreign keys=true;datetimeformat=CurrentCulture;"
                })
            {
                connection.Open();
                var sql = "SELECT * FROM WeatherRecords";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {







                        var record = new WeatherRecord();
                        DateTime dt;
                        if (DateTime.TryParse(reader["timestamp"].ToString(), out dt))
                        {
                            record.Station = station;
                            record.Id = Convert.ToInt64(reader["id"]);
                            record.TimeStamp = dt;
                        }
                        records.Add(record);

                    }
                    return records;
                }
            }
        }














        public List<ISensorValue> GetSensorValuesForRecordId(long id)
        {
            return null;
            //var records = new List<WeatherRecord>();
            //using (
            //     var connection = new SQLiteConnection
            //     {
            //         ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;datetimeformat=CurrentCulture;"
            //     })
            //{
            //    connection.Open();
            //    var sql = "SELECT * FROM WeatherRecords LEFT JOIN SensorValues on sensorvalue_id = SensorValues.Id LEFT JOIN Sensors ON sensor_id = Sensors.Id WHERE WeatherRecords.Id = @id";
            //    using (var command = new SQLiteCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@id", id);
            //        var reader = command.ExecuteReader();

            //        while (reader.Read())
            //        {
            //            var record = new WeatherRecord();
            //            DateTime dt;
            //            if (DateTime.TryParse(reader["timestamp"].ToString(), out dt))
            //            {
            //                record.Station = station;
            //                record.Id = Convert.ToInt64(reader["id"]);
            //                record.TimeStamp = dt;

            //            }
            //            records.Add(record);

            //        }
            //        return records;
            //    }
            //}
        }

        public long AddStation(WeatherStation station)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql =
                    "INSERT INTO stations (manufacturer, model, latitude, longitude) VALUES (@manufacturer, @model, @latitude, @longitude)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@manufacturer", station.Manufacturer);
                    command.Parameters.AddWithValue("@model", station.Model);
                    command.Parameters.AddWithValue("@latitude", station.Latitude);
                    command.Parameters.AddWithValue("@longitude", station.Longitude);
                    command.ExecuteNonQuery();

                    var sql2 = "SELECT last_insert_rowid();";
                    var command2 = new SQLiteCommand(sql2, connection);
                    var id = command2.ExecuteScalar();
                    return (long)id;
                }
            }
        }

        public List<ISensor> GetSensorsForStation(WeatherStation station)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sensors = new List<ISensor>();
                var sql =
                    "SELECT * FROM sensors LEFT JOIN unittypes ON sensors.unittype_id = unittypes.id WHERE station_id = @stationid";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@stationid", station.Id);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var typ = reader[6].ToString();
                        var pp = (Enums.UnitType)System.Enum.Parse(typeof(Enums.UnitType), typ);

                        var sensor = new Sensor
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString(),
                            Station = GetStationById(Convert.ToInt32(reader["station_id"])),
                            Correction = Convert.ToDouble(reader["correction"].ToString()),
                            Type = pp
                        };
                        sensors.Add(sensor);
                    }
                    return sensors;
                }
            }
        }

        public void CreateTables()
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql = @"CREATE TABLE [Stations] (
    [id] integer NOT NULL PRIMARY KEY AUTOINCREMENT,

    [manufacturer] nvarchar(254),

    [model] nvarchar(254),
	[latitude]
        double,
	[longitude]
        double
);

CREATE TABLE[UnitTypes] (

    [id]
        integer NOT NULL PRIMARY KEY AUTOINCREMENT,

   [name] nvarchar(254)
);

CREATE TABLE[Sensors] (

    [id]
        integer NOT NULL PRIMARY KEY AUTOINCREMENT,

   [name] nvarchar(254),
	[unittype_id]
        integer NOT NULL,
	[weatherstation_id]
        integer NOT NULL,
	[correction]
        double NOT NULL,
	FOREIGN KEY([unittype_id])

        REFERENCES[UnitTypes] ([id])
		ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY([weatherstation_id])

        REFERENCES[Stations] ([id])
		ON UPDATE CASCADE ON DELETE CASCADE
);

        CREATE TABLE[SensorValues] (

    [id]
        integer NOT NULL PRIMARY KEY AUTOINCREMENT,

   [value] double,
	[sensor_id]
        integer NOT NULL,
	FOREIGN KEY([sensor_id])

        REFERENCES[Sensors] ([id])
		ON UPDATE CASCADE ON DELETE CASCADE
);

        CREATE TABLE[Units] (

    [id]
        integer NOT NULL PRIMARY KEY AUTOINCREMENT,

   [unittype_id] integer,
	[displayname] nvarchar(254),
	[displayunit] nvarchar(254)
);

CREATE TABLE[WeatherRecords] (

    [id]
        integer NOT NULL PRIMARY KEY AUTOINCREMENT,

   [timestamp] datetime,
	[sensorvalue_id]
        integer,
	FOREIGN KEY([sensorvalue_id])

        REFERENCES[SensorValues] ([id])
		ON UPDATE CASCADE ON DELETE CASCADE
);
";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    var id = command.ExecuteNonQuery();
                }
            }
        }

        public WeatherStation GetStationById(long id)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                WeatherStation station = new WeatherStation();
                var sql = "SELECT * FROM stations WHERE Id = @id";
                using (var command = new SQLiteCommand(sql, connection))
                {
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
            }
        }

        public void DeleteStation(WeatherStation station)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql = "DELETE FROM stations WHERE Id = @id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", station.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public long Update(WeatherStation station)
        {
            using (
                var connection = new SQLiteConnection
                {
                    ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
                })
            {
                connection.Open();

                var sql =
                    "UPDATE stations SET manufacturer=@manufacturer, model=@model, longitude=@longitude, latitude=@latitude WHERE Id = @id";
                using (var command = new SQLiteCommand(sql, connection))
                {
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
    }
}