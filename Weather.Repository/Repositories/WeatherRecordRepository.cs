using System;
using System.Collections.Generic;
using System.Data.SQLite;
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


        public async Task<List<IWeatherRecord>> GetAllForStation(int weatherStationId, DateTime startDate,
            DateTime endDate)
        {
            var WeatherRecords = new List<IWeatherRecord>();
            var allWeatherStations = _weatherStationRepository.GetAllWeatherStations();
            var allSensorTypes = _sensorTypeRepository.GetAll();

            _log.Debug("WeatherRecordRepository.GetAllForStation();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherRecordId = 0,
                Timestamp = new DateTime(),
                WeatherStationId = 0,
                Id = 0,
                wrsvWeatherRecordId = 0,
                wrsvSensorValueId = 0,
                SensorValueId = 0,
                RawValue = (double?) 0,
                SensorId = 0,
                sSensorId = 0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                SensorTypeId = 0
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
                                        WeatherRecordId = Convert.ToInt32(reader["WeatherRecordId"]),
                                        Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                                        WeatherStationId = Convert.ToInt32(reader["WeatherStationId"]),
                                        Id = Convert.ToInt32(reader["Id"]),
                                        wrsvWeatherRecordId = Convert.ToInt32(reader["wrsvWeatherRecordId"]),
                                        wrsvSensorValueId = Convert.ToInt32(reader["wrsvSensorValueId"]),
                                        SensorValueId = Convert.ToInt32(reader["SensorValueId"]),
                                        RawValue = DbUtils.ParseDoubleNull(reader["RawValue"].ToString()),
                                        SensorId = Convert.ToInt32(reader["SensorId"]),
                                        sSensorId = Convert.ToInt32(reader["sSensorId"]),
                                        Manufacturer = reader["Manufacturer"].ToString(),
                                        Model = reader["Model"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        SensorTypeId = Convert.ToInt32(reader["SensorTypeId"])
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
                var sensors =
                    mappedReader.GroupBy(
                        x => new {x.sSensorId, x.Manufacturer, x.Model, x.Description, x.SensorTypeId}, x => x,
                        (key, g) => new Sensor
                        {
                            SensorId = key.sSensorId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Description = key.Description,
                            SensorType = allSensorTypes.FirstOrDefault(x => x.SensorTypeId == key.SensorTypeId)
                        }).ToList();

                var sensorValues =
                    mappedReader.GroupBy(x => new {x.SensorValueId, x.RawValue, x.SensorId}, x => x,
                        (key, g) => new SensorValue
                        {
                            SensorValueId = key.SensorValueId,
                            RawValue = key.RawValue,
                            SensorId = key.SensorId,
                            Sensor = sensors.FirstOrDefault(x => x.SensorId == key.SensorId)
                        }).ToList();

                var stations = mappedReader
                    .GroupBy(
                        x =>
                            new
                            {
                                x.WeatherRecordId,
                                x.Timestamp,
                                x.WeatherStationId,
                                x.Id,
                                x.wrsvWeatherRecordId,
                                x.wrsvSensorValueId
                            }, x => x,
                        (key, g) =>
                            new
                            {
                                key.WeatherRecordId,
                                WeatherRecord =
                                new WeatherRecord
                                {
                                    WeatherRecordId = key.WeatherRecordId,
                                    TimeStamp = key.Timestamp,
                                    WeatherStation =
                                        allWeatherStations.FirstOrDefault(
                                            x => x.WeatherStationId == key.WeatherStationId),
                                    SensorValues =
                                        sensorValues.Where(x => x.SensorValueId == key.wrsvSensorValueId)
                                            .Cast<ISensorValue>()
                                            .ToList()
                                }
                            }).ToList();


                WeatherRecords =
                    stations.GroupBy(x =>
                                x.WeatherRecordId, x => x.WeatherRecord,
                        (key, g) =>
                            new WeatherRecord
                            {
                                WeatherRecordId = key,
                                SensorValues = g.SelectMany(ff => ff.SensorValues).Distinct().ToList(),
                                TimeStamp = g.Select(x => x.TimeStamp).First(),
                                WeatherStationId = g.Select(x => x.WeatherStationId).First(),
                                WeatherStation = g.Select(x => x.WeatherStation).First()
                            }).Cast<IWeatherRecord>().ToList();
            });
            return WeatherRecords;
        }

        public int Add(IWeatherRecord record)
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

        public int AddWeatherRecordSensorValue(int weatherRecordId, int sensorValueId)
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
    }

    public class Join
    {
        public int Id { get; set; }
        public int WeatherRecordId { get; set; }
        public int SensorValueId { get; set; }
        public WeatherRecord Record { get; set; }
        public SensorValue SensorValue { get; set; }
    }
}