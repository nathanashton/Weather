using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class WeatherRecordRepository : IWeatherRecordRepository
    {
        private readonly ILog _log;
        private readonly ISensorTypeRepository _sensorTypeRepository;
        private readonly ISettings _settings;
        private readonly IWeatherStationRepository _weatherStationRepository;

        public WeatherRecordRepository(ILog log, IWeatherStationRepository weatherStationRepository, ISettings settings,
            ISensorTypeRepository sensorTypeRepository)
        {
            _settings = settings;
            _log = log;
            _weatherStationRepository = weatherStationRepository;
            _sensorTypeRepository = sensorTypeRepository;
        }

        public List<IWeatherRecord> GetAll()
        {
            return null;
        }

        public async Task<List<Join>> GetAllJoins()
        {
            var Joins = new List<Join>();

            var sql = @"SELECT * FROM WeatherRecords_SensorValues";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (reader.Read())
                                {
                                    var record = new Join
                                    {
                                        Id = Convert.ToInt32(reader["Id"]),
                                        WeatherRecordId = Convert.ToInt32(reader["WeatherRecordId"]),
                                        SensorValueId = Convert.ToInt32(reader["SensorValueId"])
                                    };
                                    Joins.Add(record);
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

            return Joins;
        }

        public async Task<ObservableCollection<IWeatherRecord>> GetAllForStation(long weatherStationId, DateTime startDate,
            DateTime endDate)
        {
            var joins = await GetAllJoins();
            var t = new ObservableCollection<IWeatherRecord>();
            var weatherRecords = new List<IWeatherRecord>();
            var allWeatherStations = _weatherStationRepository.GetAllWeatherStations();
            var allSensorTypes = _sensorTypeRepository.GetAll();

            _log.Debug("WeatherRecordRepository.GetAllForStation();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherRecordId = (long)0,
                Timestamp = new DateTime(),
                WeatherStationId = (long)0,
                Id = (long)0,
                wrsvWeatherRecordId = (long)0,
                wrsvSensorValueId = (long)0,
                SensorValueId = (long)0,
                RawValue = (double?)0,
                SensorId = (long)0,
                sSensorId = (long)0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                SensorTypeId = (long)0
            }).ToList();

            var ss = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var ee = endDate.ToString("yyyy-MM-dd HH:mm:ss");

            var sql2 =
                @"SELECT
                wr.[WeatherRecordId] as WeatherRecordId,
                wr.[Timestamp] as Timestamp,
                wr.[WeatherStationId] as WeatherStationId,
                wrsv.[Id] as Id,
                wrsv.[WeatherRecordId] as wrsvWeatherRecordId,
                wrsv.[SensorValueId] as wrsvSensorValueId,
                sv.[SensorValueId] as SensorValueId,
                sv.[RawValue] as RawValue,
                sv.[SensorId] as SensorId,
                s.[SensorId] as sSensorId,
                s.[Manufacturer] as Manufacturer,
                s.[Model] as Model,
                s.[Description] as Description,
                s.[SensorTypeId] as SensorTypeId
                FROM [WeatherRecords] wr
                LEFT JOIN WeatherRecords_SensorValues wrsv ON wr.WeatherRecordId = wrsv.WeatherRecordId
                LEFT JOIN SensorValues sv ON sv.SensorValueId = wrsv.SensorValueId
                LEFT JOIN Sensors s ON s.SensorId = sv.SensorId WHERE wr.WeatherStationId = @Id AND CAST(strftime('%s', Timestamp)  AS  integer) >= CAST(strftime('%s', '" +
                ss + @"')  AS  integer) AND CAST(strftime('%s', Timestamp)  AS  integer) <= CAST(strftime('%s', '" + ee +
                @"')  AS  integer)";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql2, connection))
                        {
                            command.Parameters.AddWithValue("@Id", weatherStationId);

                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (reader.Read())
                                {
                                    mappedReader.Add(new
                                    {
                                        WeatherRecordId = Convert.ToInt64(reader["WeatherRecordId"]),
                                        Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                                        WeatherStationId = Convert.ToInt64(reader["WeatherStationId"]),
                                        Id = Convert.ToInt64(reader["Id"]),
                                        wrsvWeatherRecordId = Convert.ToInt64(reader["wrsvWeatherRecordId"]),
                                        wrsvSensorValueId = Convert.ToInt64(reader["wrsvSensorValueId"]),
                                        SensorValueId = Convert.ToInt64(reader["SensorValueId"]),
                                        RawValue = DbUtils.ParseDoubleNull(reader["RawValue"].ToString()),
                                        SensorId = Convert.ToInt64(reader["SensorId"]),
                                        sSensorId = Convert.ToInt64(reader["sSensorId"]),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        SensorTypeId = Convert.ToInt64(reader["SensorTypeId"])
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

            await Task.Run(() =>
            {
                // Get all Sensors

                var sensors = new Dictionary<long, ISensor>();
            for (int index = 0; index < mappedReader.Count; index++)
            {
                var value = mappedReader[index];
                var sensor = new Sensor
                {
                    SensorId = value.sSensorId,
                    Manufacturer = value.Manufacturer,
                    Model = value.Model,
                    Description = value.Description,
                    SensorType = allSensorTypes.FirstOrDefault(x => x.SensorTypeId == value.SensorTypeId)
                };
                if (!sensors.ContainsKey(sensor.SensorId))
                {
                    sensors.Add(sensor.SensorId, sensor);
                }
            }

            var sensorValuesList = new Dictionary<long, ISensorValue>();
            for (int index = 0; index < mappedReader.Count; index++)
            {
                var value = mappedReader[index];
                var sensorvalue = new SensorValue
                {
                    SensorValueId = value.SensorValueId,
                    RawValue = value.RawValue,
                    SensorId = value.SensorId,
                    Sensor = sensors.FirstOrDefault(x => x.Key == value.SensorId).Value
                };
                if (!sensorValuesList.ContainsKey(sensorvalue.SensorValueId))
                {
                    sensorValuesList.Add(sensorvalue.SensorValueId, sensorvalue);
                }
            }

            var stationsList = new Dictionary<long, IWeatherRecord>();
            for (int index = 0; index < mappedReader.Count; index++)
            {
                var value = mappedReader[index];
                var joinRecord = joins.Where(x => x.WeatherRecordId == value.WeatherRecordId);
                var sensorValues = joinRecord.Select(j => sensorValuesList.FirstOrDefault(x => x.Key == j.SensorValueId).Value);

                var record = new WeatherRecord
                {
                    WeatherRecordId = value.WeatherRecordId,
                    TimeStamp = value.Timestamp,
                    WeatherStation =
                        allWeatherStations.FirstOrDefault(
                            x => x.WeatherStationId == value.WeatherStationId),
                    SensorValues = sensorValues
                };

                if (!stationsList.ContainsKey(record.WeatherRecordId))
                {
                    stationsList.Add(record.WeatherRecordId, record);
                }
            }

            weatherRecords = stationsList.Values.ToList();
             t = new ObservableCollection<IWeatherRecord>(weatherRecords);
            });
            return t;
        }

        public long Add(IWeatherRecord record)
        {
            _log.Debug("WeatherRecordRepository.Add();");

            var sql = @"INSERT INTO WeatherRecords (Timestamp, WeatherStationId) VALUES (@Timestamp, @WeatherStationId)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Timestamp", record.TimeStamp);
                            command.Parameters.AddWithValue("@WeatherStationId", record.WeatherStationId);

                            command.ExecuteNonQuery();

                            var command2 = new SQLiteCommand(sql2, connection);
                            var id = command2.ExecuteScalar();
                            return Convert.ToInt64(id);
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

        public long AddWeatherRecordSensorValue(long weatherRecordId, long sensorValueId)
        {
            _log.Debug("WeatherRecordRepository.Add();");

            var sql = @"INSERT INTO WeatherRecords_SensorValues (WeatherRecordId, SensorValueId) VALUES (@WeatherRecordId, @SensorValueId)";
            var sql2 = "SELECT last_insert_rowid();";
            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@WeatherRecordId", weatherRecordId);
                            command.Parameters.AddWithValue("@SensorValueId", sensorValueId);

                            command.ExecuteNonQuery();

                            var command2 = new SQLiteCommand(sql2, connection);
                            var id = command2.ExecuteScalar();
                            return Convert.ToInt64(id);
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

        public IWeatherRecord AddRecordAndSensorValues(IWeatherRecord weatherrecord)
        {
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            //var sql = @"INSERT INTO SensorValues (RawValue, SensorId) VALUES (@RawValue, @SensorId)";
            //var sql2 = "SELECT last_insert_rowid();";

            //using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
            //{
            //    connection.Open();
            //    {
            //            // Insert every SensorValue

            //            using (var command = new SQLiteCommand(sql, connection))
            //            {
            //                using (var transaction2 = connection.BeginTransaction())
            //                {
            //                    // Insert each SensorValue
            //                    foreach (var sensorvalue in weatherrecord.SensorValues)
            //                    {
            //                        command.Parameters.AddWithValue("@RawValue", sensorvalue.RawValue);
            //                        command.Parameters.AddWithValue("@SensorId", sensorvalue.Sensor.SensorId);
            //                        command.ExecuteNonQuery();

            //                        //var command2 = new SQLiteCommand(sql2, connection);
            //                        //var id = command2.ExecuteScalar();
            //                        //sensorvalue.SensorValueId = Convert.ToInt32(id);
            //                    }

            //                transaction2.Commit();

            //                }

            //            }

            //    }
            //}
            //stopwatch.Stop();
            //var elapsed = stopwatch.ElapsedMilliseconds;
            //return weatherrecord;
            return null;
        }

        public List<IWeatherRecord> AddRecordsAndSensorValues(List<IWeatherRecord> weatherrecords)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var sql = @"INSERT INTO SensorValues (RawValue, SensorId) VALUES (@RawValue, @SensorId)";
            var sql2 = "SELECT last_insert_rowid();";

            using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
            {
                connection.Open();
                {
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        using (var transaction = connection.BeginTransaction())
                        {
                            foreach (var record in weatherrecords)
                            {
                                // Insert each SensorValue
                                foreach (var sensorvalue in record.SensorValues)
                                {
                                    command.CommandText = @"INSERT INTO SensorValues (RawValue, SensorId) VALUES (@RawValue, @SensorId)";
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@RawValue", sensorvalue.RawValue);
                                    command.Parameters.AddWithValue("@SensorId", sensorvalue.Sensor.SensorId);
                                    command.ExecuteNonQuery();

                                    command.CommandText = sql2;
                                    var id = command.ExecuteScalar();
                                    sensorvalue.SensorValueId = Convert.ToInt64(id);
                                }

                                //Insert each WeatherRecord
                                command.Parameters.Clear();
                                command.CommandText = @"INSERT INTO WeatherRecords (Timestamp, WeatherStationId) VALUES (@Timestamp, @WeatherStationId)";

                                command.Parameters.AddWithValue("@Timestamp", record.TimeStamp);
                                command.Parameters.AddWithValue("@WeatherStationId", record.WeatherStationId);
                                command.ExecuteNonQuery();

                                command.CommandText = sql2;
                                record.WeatherRecordId = Convert.ToInt64(command.ExecuteScalar());

                                //Insert WeatherRecord_SensorValue
                                command.Parameters.Clear();
                                command.CommandText = @"INSERT INTO WeatherRecords_SensorValues (WeatherRecordId, SensorValueId) VALUES (@WeatherRecordId, @SensorValueId)";

                                foreach (var sensorvalue in record.SensorValues)
                                {
                                    command.Parameters.AddWithValue("@WeatherRecordId", record.WeatherRecordId);
                                    command.Parameters.AddWithValue("@SensorValueId", sensorvalue.SensorValueId);
                                    command.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                        }
                    }
                }
            }
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            return weatherrecords;
        }
    }

    public class Join
    {
        public int Id { get; set; }
        public int WeatherRecordId { get; set; }
        public int SensorValueId { get; set; }
        public WeatherRecord Record { get; set; }
        public SensorValue SensorValue { get; set; }
    }

    public static class LinqUtilities
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
        {
            HashSet<T> hashSet = new HashSet<T>();

            foreach (var en in enumerable)
            {
                hashSet.Add(en);
            }

            return hashSet;
        }
    }
}