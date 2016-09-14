using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class WeatherStationRepository : IWeatherStationRepository
    {
        private readonly ILog _log;
        private readonly ISensorRepository _sensorRepository;
        private readonly ISettings _settings;

        public WeatherStationRepository(ILog log, ISensorRepository sensorRepository, ISettings settings)
        {
            _settings = settings;
            _log = log;
            _sensorRepository = sensorRepository;
        }

        public IWeatherStation GetById(int id)
        {
            _log.Debug("WeatherStationRepository.GetById();");
            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherStationId = 0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                Latitude = (double?) 0,
                Longitude = (double?) 0,
                SensorId = (int?) 0,
                Correction = (double) 0,
                Notes = string.Empty,
                StationSensorId = (int?) 0
            }).ToList();

            var sql = @"SELECT
                        ws.[WeatherStationId] as WeatherStationId,
						ws.[Description] as Description,
						ws.[Latitude] as Latitude,
						ws.[Longitude] as Longitude,
						ws.[Manufacturer] as Manufacturer,
						ws.[Model] as Model,
						wss.[SensorId] as SensorId,
                        wss.[Correction] as Correction,
                        wss.[Notes] as Notes,
                        wss.[Id] as StationSensorId
                        FROM [WeatherStations] ws
                        LEFT JOIN WeatherStations_Sensors wss ON ws.WeatherStationId = wss.WeatherStationId WHERE ws.WeatherStationId = @Id";
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
                                    mappedReader.Add(new
                                    {
                                        WeatherStationId = Convert.ToInt32(reader["WeatherStationId"].ToString()),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        Latitude = DbUtils.ParseDoubleNull(reader["Latitude"].ToString()),
                                        Longitude = DbUtils.ParseDoubleNull(reader["Longitude"].ToString()),
                                        SensorId = DbUtils.ParseIntNull(reader["SensorId"].ToString()),
                                        Correction =Convert.ToDouble(reader["Correction"].ToString()),
                                        Notes = reader["Notes"].ToString(),
                                        StationSensorId = DbUtils.ParseIntNull(reader["StationSensorId"].ToString())
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                _log.Error("", ex);
                throw;
            }

            var sensors =
                mappedReader.Where(x => x.SensorId != null)
                    .Select(x => new {x.WeatherStationId, x.SensorId, x.Correction, x.Notes, x.StationSensorId});

            var stations = mappedReader
                .GroupBy(
                    x =>
                        new
                        {
                            x.WeatherStationId,
                            x.Manufacturer,
                            x.Model,
                            x.Latitude,
                            x.Longitude,
                            x.Description,
                            x.SensorId
                        }, x => x,
                    (key, g) =>
                        new WeatherStation
                        {
                            WeatherStationId = key.WeatherStationId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Description = key.Description,
                            Latitude = key.Latitude,
                            Longitude = key.Longitude,
                            Sensors = new System.Collections.ObjectModel.ObservableCollection<IStationSensor>()
                        }).ToList();

            var distinctStations = stations.GroupBy(x => x.WeatherStationId, (key, group) => group.First()).ToList();

            foreach (var station in distinctStations)
            {
                var stationSensors = sensors.Where(x => x.WeatherStationId == station.WeatherStationId).ToList();
                foreach (var sensor in stationSensors)
                {
                    if (sensor.StationSensorId != null)
                    {
                        var n = new StationSensor
                        {
                            Sensor = _sensorRepository.GetById((int) sensor.SensorId),
                            Correction = sensor.Correction,
                            Notes = sensor.Notes,
                            StationSensorId = (int) sensor.StationSensorId
                        };
                        station.Sensors.Add(n);
                    }
                }
            }

            return distinctStations.First();
        }


        public IWeatherStation GetByIdShort(int id)
        {
            _log.Debug("WeatherStationRepository.GetById();");
            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherStationId = 0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                Latitude = (double?) 0,
                Longitude = (double?) 0,
                SensorId = (int?) 0,
                Correction = (double) 0,
                Notes = string.Empty,
                StationSensorId = (int?) 0
            }).ToList();

            var sql = @"SELECT
                        ws.[WeatherStationId] as WeatherStationId,
						ws.[Description] as Description,
						ws.[Latitude] as Latitude,
						ws.[Longitude] as Longitude,
						ws.[Manufacturer] as Manufacturer,
						ws.[Model] as Model,
						wss.[SensorId] as SensorId,
                        wss.[Correction] as Correction,
                        wss.[Notes] as Notes,
                        wss.[Id] as StationSensorId
                        FROM [WeatherStations] ws
                        LEFT JOIN WeatherStations_Sensors wss ON ws.WeatherStationId = wss.WeatherStationId WHERE ws.WeatherStationId = @Id";
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
                                    mappedReader.Add(new
                                    {
                                        WeatherStationId = Convert.ToInt32(reader["WeatherStationId"].ToString()),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        Latitude = DbUtils.ParseDoubleNull(reader["Latitude"].ToString()),
                                        Longitude = DbUtils.ParseDoubleNull(reader["Longitude"].ToString()),
                                        SensorId = DbUtils.ParseIntNull(reader["SensorId"].ToString()),
                                        Correction =Convert.ToDouble(reader["Correction"].ToString()),
                                        Notes = reader["Notes"].ToString(),
                                        StationSensorId = DbUtils.ParseIntNull(reader["StationSensorId"].ToString())
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                _log.Error("", ex);
                throw;
            }

            var sensors =
                mappedReader.Where(x => x.SensorId != null)
                    .Select(x => new {x.WeatherStationId, x.SensorId, x.Correction, x.Notes, x.StationSensorId});

            var stations = mappedReader
                .GroupBy(
                    x =>
                        new
                        {
                            x.WeatherStationId,
                            x.Manufacturer,
                            x.Model,
                            x.Latitude,
                            x.Longitude,
                            x.Description,
                            x.SensorId
                        }, x => x,
                    (key, g) =>
                        new WeatherStation
                        {
                            WeatherStationId = key.WeatherStationId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Description = key.Description,
                            Latitude = key.Latitude,
                            Longitude = key.Longitude,
                            Sensors = new System.Collections.ObjectModel.ObservableCollection<IStationSensor>()
                        }).ToList();

            var distinctStations = stations.GroupBy(x => x.WeatherStationId, (key, group) => group.First()).ToList();

            foreach (var station in distinctStations)
            {
                var stationSensors = sensors.Where(x => x.WeatherStationId == station.WeatherStationId).ToList();
                foreach (var sensor in stationSensors)
                {
                    if (sensor.StationSensorId != null)
                    {
                        var n = new StationSensor
                        {
                            Sensor = new Sensor {SensorId = (int) sensor.SensorId},
                            Correction = sensor.Correction,
                            Notes = sensor.Notes,
                            StationSensorId = (int) sensor.StationSensorId
                        };
                        station.Sensors.Add(n);
                    }
                }
            }

            return distinctStations.First();
        }

        public int Add(IWeatherStation station)
        {
            _log.Debug("WeatherStationRepository.Add();");

            var sql =
                @"INSERT INTO WeatherStations (Manufacturer, Model, Description, Latitude, Longitude) VALUES (@Manufacturer, @Model, @Description, @Latitude, @Longitude)";
            var sql2 = "SELECT last_insert_rowid();";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Manufacturer", station.Manufacturer);
                            command.Parameters.AddWithValue("@Model", station.Model);
                            command.Parameters.AddWithValue("@Description", station.Description);
                            command.Parameters.AddWithValue("@Latitude", station.Latitude);
                            command.Parameters.AddWithValue("@Longitude", station.Longitude);
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

        public void AddSensorToStation(IStationSensor sensor, IWeatherStation station)
        {
            _log.Debug("WeatherStationRepository.AddSensorToStation();");

            var sql =
                @"INSERT INTO WeatherStations_Sensors (WeatherStationId, SensorId, Correction, Notes) VALUES (@WeatherStationId, @SensorId, @Correction, @Notes)";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@WeatherStationId", station.WeatherStationId);
                            command.Parameters.AddWithValue("@SensorId", sensor.Sensor.SensorId);
                            command.Parameters.AddWithValue("@Correction", sensor.Correction);
                            command.Parameters.AddWithValue("@Notes", sensor.Notes);

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

        public bool AnyStationUsesSensor(ISensor sensor)
        {
            _log.Debug("WeatherStationRepository.AnyStationUsesSensor();");

            var sql = @"SELECT * FROM WeatherStations_Sensors WHERE SensorId = @SensorId";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@SensorId", sensor.SensorId);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    return true;
                                }
                                return false;
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
        }

        public void Delete(int id)
        {
            _log.Debug("WeatherStationRepository.Delete();");

            var sql = @"DELETE FROM WeatherStations WHERE WeatherStationId = @Id";
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

        public List<IWeatherStation> GetAllWeatherStations()
        {
            _log.Debug("WeatherStationRepository.GetAllWeatherStations();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherStationId = 0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                Latitude = (double?) 0,
                Longitude = (double?) 0,
                SensorId = (int?) 0,
                Correction = (double?) 0,
                Notes = string.Empty,
                StationSensorId = (int?) 0
            }).ToList();

            var sql = @"SELECT
                        ws.[WeatherStationId] as WeatherStationId,
						ws.[Description] as Description,
						ws.[Latitude] as Latitude,
						ws.[Longitude] as Longitude,
						ws.[Manufacturer] as Manufacturer,
						ws.[Model] as Model,
						wss.[SensorId] as SensorId,
                        wss.[Correction] as Correction,
                        wss.[Notes] as Notes,
                        wss.[Id] as StationSensorId
                        FROM [WeatherStations] ws
                        LEFT JOIN WeatherStations_Sensors wss ON ws.WeatherStationId = wss.WeatherStationId";
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
                                        WeatherStationId = Convert.ToInt32(reader["WeatherStationId"].ToString()),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        Latitude = DbUtils.ParseDoubleNull(reader["Latitude"].ToString()),
                                        Longitude = DbUtils.ParseDoubleNull(reader["Longitude"].ToString()),
                                        SensorId = DbUtils.ParseIntNull(reader["SensorId"].ToString()),
                                        Correction = DbUtils.ParseDoubleNull(reader["Correction"].ToString()),
                                        Notes = reader["Notes"].ToString(),
                                        StationSensorId = DbUtils.ParseIntNull(reader["StationSensorId"].ToString())
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                _log.Error("", ex);
                throw;
            }

            var sensors =
                mappedReader.Where(x => x.SensorId != null)
                    .Select(x => new {x.WeatherStationId, x.SensorId, x.Correction, x.Notes, x.StationSensorId});

            var stations = mappedReader
                .GroupBy(
                    x =>
                        new
                        {
                            x.WeatherStationId,
                            x.Manufacturer,
                            x.Model,
                            x.Latitude,
                            x.Longitude,
                            x.Description,
                            x.SensorId
                        }, x => x,
                    (key, g) =>
                        new WeatherStation
                        {
                            WeatherStationId = key.WeatherStationId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Description = key.Description,
                            Latitude = key.Latitude,
                            Longitude = key.Longitude,
                            Sensors = new System.Collections.ObjectModel.ObservableCollection<IStationSensor>()
                        }).ToList();

            var distinctStations = stations.GroupBy(x => x.WeatherStationId, (key, group) => group.First()).ToList();

            foreach (var station in distinctStations)
            {
                var stationSensors = sensors.Where(x => x.WeatherStationId == station.WeatherStationId).ToList();
                foreach (var sensor in stationSensors)
                {
                    if (sensor.StationSensorId != null)
                    {
                        var n = new StationSensor
                        {
                            Sensor = _sensorRepository.GetById((int) sensor.SensorId),
                            Correction = (double) sensor.Correction,
                            Notes = sensor.Notes,
                            StationSensorId = (int) sensor.StationSensorId
                        };
                        station.Sensors.Add(n);
                    }
                }
            }

            return distinctStations.Cast<IWeatherStation>().ToList();
        }

        public void RemoveSensorFromStation(IStationSensor sensor, IWeatherStation station)
        {
            _log.Debug("WeatherStationRepository.RemoveSensorFromStation();");

            var sql = @"DELETE FROM WeatherStations_Sensors WHERE Id = @Id";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", sensor.StationSensorId);

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

        public void Update(IWeatherStation station)
        {
            _log.Debug("WeatherStationRepository.Update();");

            var sql =
                @"UPDATE WeatherStations SET Manufacturer = @Manufacturer, Model = @Model, Description = @Description, Latitude = @Latitude, Longitude = @Longitude WHERE WeatherStationId = @Id";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Manufacturer", station.Manufacturer);
                            command.Parameters.AddWithValue("@Model", station.Model);
                            command.Parameters.AddWithValue("@Description", station.Description);
                            command.Parameters.AddWithValue("@Latitude", station.Latitude);
                            command.Parameters.AddWithValue("@Longitude", station.Longitude);
                            command.Parameters.AddWithValue("@Id", station.WeatherStationId);
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