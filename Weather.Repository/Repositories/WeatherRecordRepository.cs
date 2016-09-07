using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class WeatherRecordRepository : IWeatherRecordRepository
    {
       
        private readonly ILog _log;
        private readonly ISensorValueRepository _sensorValueRepository;
        private readonly IWeatherStationRepository _weatherStationRepository;
        private readonly ISettings _settings;

        public WeatherRecordRepository(ILog log, IWeatherStationRepository weatherStationRepository,
            ISensorValueRepository sensorValueRepository, ISettings settings)
        {
            _settings = settings;
            _log = log;
            _weatherStationRepository = weatherStationRepository;
            _sensorValueRepository = sensorValueRepository;
        }

        public List<IWeatherRecord> GetAll()
        {
            _log.Debug("WeatherRecordRepository.GetAll();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherRecordId = 0,
                Timestamp = new DateTime(),
                WeatherStationId = 0,
                Id = 0,
                wrsvWeatherRecordId = 0,
                wrsvSensorValueId = 0
            }).ToList();

            var sql = @"SELECT
			        wr.[WeatherRecordId] as WeatherRecordId,
			        wr.[Timestamp] as Timestamp,
			        wr.[WeatherStationId] as WeatherStationId,
			        wrsv.[Id] as Id,
			        wrsv.[WeatherRecordId] as wrsvWeatherRecordId,
			        wrsv.[SensorValueId] as wrsvSensorValueId
			        FROM [WeatherRecords] wr
			        LEFT JOIN WeatherRecords_SensorValues wrsv ON wr.WeatherRecordId = wrsv.WeatherRecordId";

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
                                        WeatherRecordId = Convert.ToInt32(reader["WeatherRecordId"]),
                                        Timestamp = Convert.ToDateTime(reader["Timestamp"]),
                                        WeatherStationId = Convert.ToInt32(reader["WeatherStationId"]),
                                        Id = Convert.ToInt32(reader["Id"]),
                                        wrsvWeatherRecordId = Convert.ToInt32(reader["wrsvWeatherRecordId"]),
                                        wrsvSensorValueId = Convert.ToInt32(reader["wrsvSensorValueId"])
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

            var sensorValues =
                mappedReader
                    .Select(
                        x => new {x.WeatherRecordId, SensorValue = _sensorValueRepository.GetById(x.wrsvSensorValueId)})
                    .ToList();

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
                                WeatherStation = _weatherStationRepository.GetById(key.WeatherStationId),
                                SensorValues = new List<ISensorValue>()
                            }
                        }).ToList();

            var distinctrecords = stations.GroupBy(x => x.WeatherRecordId, (key, group) => group.First()).ToList();
            foreach (var record in distinctrecords)
            {
                record.WeatherRecord.SensorValues =
                    sensorValues.Where(x => x.WeatherRecordId == record.WeatherRecordId)
                        .Select(y => y.SensorValue)
                        .ToList();
            }

            return distinctrecords.Select(y => y.WeatherRecord).Cast<IWeatherRecord>().ToList();
        }

        public List<IWeatherRecord> GetAllForStation(int weatherStationId, DateTime startDate, DateTime endDate)
        {
            _log.Debug("WeatherRecordRepository.GetAllForStation();");

            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherRecordId = 0,
                Timestamp = new DateTime(),
                WeatherStationId = 0,
                Id = 0,
                wrsvWeatherRecordId = 0,
                wrsvSensorValueId = 0
            }).ToList();

            var ss = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var ee = endDate.ToString("yyyy-MM-dd HH:mm:ss");

            var sql =
                @"SELECT wr.[WeatherRecordId] as WeatherRecordId,wr.[Timestamp] as Timestamp,wr.[WeatherStationId] as WeatherStationId,wrsv.[Id] as Id,wrsv.[WeatherRecordId] as wrsvWeatherRecordId,wrsv.[SensorValueId] as  wrsvSensorValueId FROM [WeatherRecords] wr LEFT JOIN WeatherRecords_SensorValues wrsv ON wr.WeatherRecordId = wrsv.WeatherRecordId WHERE wr.WeatherStationId = @Id AND CAST(strftime('%s', Timestamp)  AS  integer) >= CAST(strftime('%s', '" +
                ss + @"')  AS  integer) AND CAST(strftime('%s', Timestamp)  AS  integer) <= CAST(strftime('%s', '" + ee +
                @"')  AS  integer)";

            try
            {
                using (var connection = new SQLiteConnection(_settings.DatabaseConnectionString))
                {
                    connection.Open();
                    {
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Id", weatherStationId);

                            using (var reader = command.ExecuteReader())
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
                                        wrsvSensorValueId = Convert.ToInt32(reader["wrsvSensorValueId"])
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

            var sensorValues =
                mappedReader
                    .Select(
                        x => new {x.WeatherRecordId, SensorValue = _sensorValueRepository.GetById(x.wrsvSensorValueId)})
                    .ToList();

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
                                WeatherStation = _weatherStationRepository.GetByIdShort(key.WeatherStationId),
                                SensorValues = new List<ISensorValue>()
                            }
                        }).ToList();

            var distinctrecords = stations.GroupBy(x => x.WeatherRecordId, (key, group) => group.First()).ToList();
            foreach (var record in distinctrecords)
            {
                record.WeatherRecord.SensorValues =
                    sensorValues.Where(x => x.WeatherRecordId == record.WeatherRecordId)
                        .Select(y => y.SensorValue)
                        .ToList();
                foreach (var value in record.WeatherRecord.SensorValues)
                {
                    value.Station = record.WeatherRecord.WeatherStation;
                }
            }

            return distinctrecords.Select(y => y.WeatherRecord).Cast<IWeatherRecord>().ToList();
        }
    }
}