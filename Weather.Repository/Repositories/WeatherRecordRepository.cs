using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;

namespace Weather.Repository.Repositories
{
    public class WeatherRecordRepository : IWeatherRecordRepository
    {
        private const string DbConnectionString = @"Data Source=weather.sqlite;Version=3;foreign keys=true;";
        private readonly ILog _log;

        public WeatherRecordRepository(ILog log)
        {
            _log = log;
        }

        public async Task<IEnumerable<IWeatherRecord>> GetAllWeatherRecordsAsync()
        {
            var records = new List<IWeatherRecord>();
            var sql = @"SELECT wr.[WeatherRecordId] as WeatherRecordId
                    , wr.[Timestamp] as WeatherRecordTimestamp
					,wr.[SensorValueId] as WeatherRecordSensorValueId
					,wr.[WeatherStationId] as WeatherRecordWeatherStationId
					,sv.[SensorValueId] as SensorValueId
					,sv.[RawValue] as SensorValueRawValue
					,sv.[SensorId] as SensorValueSensorId
                    FROM[WeatherRecords] wr
                   LEFT JOIN SensorValues sv ON wr.SensorValueId = sv.SensorValueId";
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
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
                                        WeatherRecordId = Convert.ToInt32(reader["WeatherRecordId"].ToString()),
                                        TimeStamp = Convert.ToDateTime(reader["Timestamp"])
                                    };
                                    records.Add(record);
                                }
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                _log.Error("", ex);
            }
            return records;
        }

        public Task<IEnumerable<IWeatherRecord>> GetWeatherRecordsForStationAsync(WeatherStation station)
        {
            throw new NotImplementedException();
        }

        public void AddWeatherRecord(IWeatherRecord record)
        {
            throw new NotImplementedException();
        }

        public void DeleteWeatherRecord(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateWeatherRecord(IWeatherRecord record)
        {
            throw new NotImplementedException();
        }
    }
}
