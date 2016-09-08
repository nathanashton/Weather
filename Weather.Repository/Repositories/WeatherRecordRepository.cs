using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class WeatherRecordRepository : IWeatherRecordRepository
    {
        private readonly ILog _log;
        private readonly IWeatherStationRepository _weatherStationRepository;
        private readonly ISettings _settings;
        private readonly ISensorTypeRepository _sensorTypeRepository;

        public WeatherRecordRepository(ILog log, IWeatherStationRepository weatherStationRepository, ISettings settings, ISensorTypeRepository sensorTypeRepository)
        {
            _settings = settings;
            _log = log;
            _weatherStationRepository = weatherStationRepository;
            _sensorTypeRepository = sensorTypeRepository;
        }

        public List<IWeatherRecord> GetAll()
        {
            // _log.Debug("WeatherRecordRepository.GetAll();");

            // var mappedReader = Enumerable.Empty<object>().Select(r => new
            // {
            //     WeatherRecordId = 0,
            //     Timestamp = new DateTime(),
            //     WeatherStationId = 0,
            //     Id = 0,
            //     wrsvWeatherRecordId = 0,
            //     wrsvSensorValueId = 0
            // }).ToList();

            // var sql = @"SELECT
            //wr.[WeatherRecordId] as WeatherRecordId,
            //wr.[Timestamp] as Timestamp,
            //wr.[WeatherStationId] as WeatherStationId,
            //wrsv.[Id] as Id,
            //wrsv.[WeatherRecordId] as wrsvWeatherRecordId,
            //wrsv.[SensorValueId] as wrsvSensorValueId
            //FROM [WeatherRecords] wr
            //LEFT JOIN WeatherRecords_SensorValues wrsv ON wr.WeatherRecordId = wrsv.WeatherRecordId";

            // try
            // {
            //     using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
            //     {
            //         connection.Open();
            //         {
            //             using (var command = new SQLiteCommand(sql, connection))
            //             {
            //                 using (var reader = command.ExecuteReader())
            //                 {
            //                     while (reader.Read())
            //                     {
            //                         mappedReader.Add(new
            //                         {
            //                             WeatherRecordId = Convert.ToInt32(reader["WeatherRecordId"]),
            //                             Timestamp = Convert.ToDateTime(reader["Timestamp"]),
            //                             WeatherStationId = Convert.ToInt32(reader["WeatherStationId"]),
            //                             Id = Convert.ToInt32(reader["Id"]),
            //                             wrsvWeatherRecordId = Convert.ToInt32(reader["wrsvWeatherRecordId"]),
            //                             wrsvSensorValueId = Convert.ToInt32(reader["wrsvSensorValueId"])
            //                         });
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // }
            // catch (SQLiteException ex)
            // {
            //     _log.Error("", ex);
            //     throw;
            // }

            // var sensorValues =
            //     mappedReader
            //         .Select(
            //             x => new {x.WeatherRecordId, SensorValue = _sensorValueRepository.GetById(x.wrsvSensorValueId)})
            //         .ToList();

            // var stations = mappedReader
            //     .GroupBy(
            //         x =>
            //             new
            //             {
            //                 x.WeatherRecordId,
            //                 x.Timestamp,
            //                 x.WeatherStationId,
            //                 x.Id,
            //                 x.wrsvWeatherRecordId,
            //                 x.wrsvSensorValueId
            //             }, x => x,
            //         (key, g) =>
            //             new
            //             {
            //                 key.WeatherRecordId,
            //                 WeatherRecord =
            //                 new WeatherRecord
            //                 {
            //                     WeatherRecordId = key.WeatherRecordId,
            //                     TimeStamp = key.Timestamp,
            //                     WeatherStation = _weatherStationRepository.GetById(key.WeatherStationId),
            //                     SensorValues = new List<ISensorValue>()
            //                 }
            //             }).ToList();

            // var distinctrecords = stations.GroupBy(x => x.WeatherRecordId, (key, group) => group.First()).ToList();
            // foreach (var record in distinctrecords)
            // {
            //     record.WeatherRecord.SensorValues =
            //         sensorValues.Where(x => x.WeatherRecordId == record.WeatherRecordId)
            //             .Select(y => y.SensorValue)
            //             .ToList();
            // }
            return null;
            //    return distinctrecords.Select(y => y.WeatherRecord).Cast<IWeatherRecord>().ToList();
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

        public async Task<List<IWeatherRecord>> GetAllTestAsync()
        {
            var Records = new List<IWeatherRecord>();

            var sql = @"SELECT * FROM WeatherRecords";

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
                                    var record = new WeatherRecord
                                    {
                                        WeatherRecordId = Convert.ToInt32(reader["WeatherRecordId"]),
                                        TimeStamp = Convert.ToDateTime(reader["Timestamp"]),
                                        SensorValues = null,
                                        WeatherStation = null,
                                        WeatherStationId = Convert.ToInt32(reader["WeatherStationId"])
                                    };
                                    Records.Add(record);
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

            return Records;
        }

        public async Task<List<IWeatherRecord>> GetAllForStation(int weatherStationId, DateTime startDate, DateTime endDate, Action callback)
        {
            List<IWeatherRecord> WeatherRecords = new List<IWeatherRecord>();
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
                RawValue = (double?)0,
                SensorId = 0,
                sSensorId = 0,
                Manufacturer = string.Empty,
                Model = string.Empty,
                Description = string.Empty,
                SensorTypeId = 0
            }).ToList();

            var ss = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var ee = endDate.ToString("yyyy-MM-dd HH:mm:ss");

            var sql2 = @"SELECT
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

            await Task.Run(() => {
                var sensors =
  mappedReader.GroupBy(x => new { x.sSensorId, x.Manufacturer, x.Model, x.Description, x.SensorTypeId }, x => x,
      (key, g) => new Sensor
      {
          SensorId = key.sSensorId,
          Manufacturer = key.Manufacturer,
          Model = key.Model,
          Description = key.Description,
          SensorType = allSensorTypes.FirstOrDefault(x => x.SensorTypeId == key.SensorTypeId)
      }).ToList();

                var sensorValues =
                    mappedReader.GroupBy(x => new { x.SensorValueId, x.RawValue, x.SensorId }, x => x,
                        (key, g) => new SensorValue
                        {
                            SensorValueId = key.SensorValueId,
                            RawValue = key.RawValue,
                            SensorId = key.SensorId,
                            Sensor = sensors.FirstOrDefault(x => x.SensorId == key.SensorId),
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
                                    WeatherStation = allWeatherStations.FirstOrDefault(x => x.WeatherStationId == key.WeatherStationId),
                                    SensorValues = sensorValues.Where(x => x.SensorValueId == key.wrsvSensorValueId).Cast<ISensorValue>().ToList()
                                }
                            }).ToList();

                var distinctrecords = stations.GroupBy(x => x.WeatherRecordId, (key, group) => group.First()).ToList();


                WeatherRecords = distinctrecords.Select(y => y.WeatherRecord).Cast<IWeatherRecord>().ToList();
                callback();
            });
            return WeatherRecords;
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