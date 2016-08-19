using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Repository.Interfaces;
using static System.String;

namespace Weather.Repository.Repositories
{
    public class WeatherStationRepository : IWeatherStationRepository
    {
        private const string DbConnectionString = @"Data Source=weather.sqlite;Version=3;foreign keys=true;";
        private readonly ILog _log;

        public WeatherStationRepository(ILog log)
        {
            _log = log;
        }

        public void GetAllWeatherStations()
        {
            throw new NotImplementedException();
        }

        public void GetWeatherStationById(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteWeatherStation(int id)
        {
            throw new NotImplementedException();
        }

        public void GetAllWeatherStationsWithSensors()
        {
            throw new NotImplementedException();
        }

        public void GetWeatherStationWithSensorsById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IWeatherStation>> GetAllWeatherStationsWithSensorsAndRecordsAsync()
        {
    //        var mappedReader = Enumerable.Empty<object>().Select(r => new
    //        {
    //            WeatherStationId = 0,
    //            Manufacturer = Empty,
    //            Model = Empty,
    //            Latitude = (double?)null,
    //            Longitude = (double?)null,

    //            WeatherRecordId = (int?)null,
    //            WeatherRecordTimestamp  = (DateTime?)null,
    //            WeatherRecordSensorValueId = (int?)null,
    //            WeatherRecordWeatherStationId = (int?)null,
                
    //            SensorValueId = (int?)null,
    //            SensorValueRawValue = (double?)null,
    //            SensorValueSensorId = (int?)null,

    //            SensorWeatherStationId = (int?)null,
    //            SensorId = (int?)null,
    //            SensorCorrection = (double?)null,
    //            SensorName = Empty,
    //            SensorUnitTypeId = (int?)null,

    //            UnitTypeId = (int?)null,
    //            UnitTypeName = Empty
    //        }).ToList();

    //        var sql = @"SELECT ws.[WeatherStationId] as WeatherStationId
    //                ,ws.[Manufacturer] as Manufacturer
		  //          ,ws.[Model] as Model
		  //          ,ws.[Latitude] as Latitude
		  //          ,ws.[Longitude] as Longitude
				//	,wr.[WeatherRecordId] as WeatherRecordId
				//	,wr.[Timestamp] as WeatherRecordTimestamp
				//	,wr.[SensorValueId] as WeatherRecordSensorValueId
				//	,wr.[WeatherStationId] as WeatherRecordWeatherStationId
				//	,sv.[SensorValueId] as SensorValueId
				//	,sv.[RawValue] as SensorValueRawValue
				//	,sv.[SensorId] as SensorValueSensorId
				//	,sr.[SensorId] as SensorId
		  //          ,sr.[Name] as SensorName
				//	,sr.[UnitTypeId] as SensorUnitTypeId
    //                ,sr.[WeatherStationId] as SensorWeatherStationId
    //                ,sr.[Correction] as SensorCorrection
				//	,ut.[UnitTypeId] as UnitTypeId
				//	,ut.[Name] as UnitTypeName

    //            FROM [WeatherStations] ws
				//LEFT JOIN WeatherRecords wr ON wr.WeatherStationId = ws.WeatherStationId
				//LEFT JOIN SensorValues sv ON wr.SensorValueId = sv.SensorValueId
				//LEFT JOIN Sensors sr ON sv.SensorId = sr.SensorId
				//LEFT JOIN UnitTypes ut ON sr.UnitTypeId = ut.UnitTypeId";
    //        using (var connection = new SQLiteConnection(DbConnectionString))
    //        {
    //            connection.Open();
    //            {
    //                using (var command = new SQLiteCommand(sql, connection))
    //                {
    //                    using (IDataReader reader = await command.ExecuteReaderAsync())
    //                    {
    //                        while (reader.Read())
    //                        {
    //                            mappedReader.Add(new
    //                            {
    //                                WeatherStationId = Convert.ToInt32(reader["WeatherStationId"].ToString()),
    //                                Manufacturer = reader["Manufacturer"].ToString(),
    //                                Model = reader["Model"].ToString(),
    //                                Latitude = DbUtils.ParseDoubleNull(reader["Latitude"].ToString()),
    //                                Longitude = DbUtils.ParseDoubleNull(reader["Longitude"].ToString()),


    //                                WeatherRecordId = DbUtils.ParseIntNull(reader["WeatherRecordId"].ToString()),
    //                                WeatherRecordTimestamp = DbUtils.ParseDateTimeNull(reader["WeatherRecordTimestamp"].ToString()),
    //                                WeatherRecordSensorValueId = DbUtils.ParseIntNull(reader["WeatherRecordSensorValueId"].ToString()),
    //                                WeatherRecordWeatherStationId = DbUtils.ParseIntNull(reader["WeatherRecordWeatherStationId"].ToString()),


    //                                SensorValueId = DbUtils.ParseIntNull(reader["SensorValueId"].ToString()),
    //                                SensorValueRawValue = DbUtils.ParseDoubleNull(reader["SensorValueRawValue"].ToString()),
    //                                SensorValueSensorId = DbUtils.ParseIntNull(reader["SensorValueSensorId"].ToString()),


    //                                SensorWeatherStationId = DbUtils.ParseIntNull(reader["SensorWeatherStationId"].ToString()),
    //                                SensorId = DbUtils.ParseIntNull(reader["SensorId"].ToString()),
    //                                SensorCorrection = DbUtils.ParseDoubleNull(reader["SensorCorrection"].ToString()),
    //                                SensorName = reader["SensorName"].ToString(),
    //                                SensorUnitTypeId = DbUtils.ParseIntNull(reader["SensorUnitTypeId"].ToString()),



    //                                UnitTypeId = DbUtils.ParseIntNull(reader["UnitTypeId"].ToString()),
    //                                UnitTypeName = reader["UnitTypeName"].ToString()
    //                            });
    //                        }
    //                    }
    //                }
    //            }
    //        }



    //        var sensorValues = mappedReader.Where(r => r.SensorValueId != null)
    //                .GroupBy(x => new { x.SensorValueId, x.SensorValueRawValue, x.SensorValueSensorId }, x => x,
    //                    (key, g) =>
    //                        new
    //                        {
    //                            key.SensorValueId,
    //                            SensorValue =
    //                                new SensorValue
    //                                {
    //                                    SensorValueId = (int)key.SensorValueId,
    //                                    RawValue = (int)key.SensorValueRawValue,
    //                                    SensorId = (int)key.SensorValueSensorId
    //                                }
    //                        }).ToList();

    //        // Sensors with a null SensorId are not attached to a station
    //        var attachedSensors =
    //            mappedReader.Where(r => r.SensorId != null)
    //                .GroupBy(x => new { x.SensorId, x.SensorName, x.WeatherStationId, x.SensorCorrection }, x => x,
    //                    (key, g) =>
    //                        new
    //                        {
    //                            key.WeatherStationId,
    //                            Sensor =
    //                                new Sensor
    //                                {
    //                                    SensorId = (int)key.SensorId,
    //                                    Name = key.SensorName,
    //                                    Correction = (double)key.SensorCorrection,
    //                                    SensorValues = sensorValues.Where(x => x.SensorValue.SensorId == key.SensorId).Select(x => x.SensorValue).Cast<ISensorValue>().ToList()
    //                                }
    //                        }).ToList();


    //        var records = mappedReader.Where(r => r.WeatherRecordId != null)
    //                .GroupBy(x => new { x.WeatherRecordId, x.WeatherRecordSensorValueId, x.WeatherRecordTimestamp, x.WeatherRecordWeatherStationId }, x => x,
    //                    (key, g) =>
    //                        new
    //                        {
    //                            key.WeatherRecordId,
    //                            WeatherRecord =
    //                                new WeatherRecord
    //                                {
    //                                    WeatherRecordId = (int)key.WeatherRecordId,
    //                                    TimeStamp = (DateTime)key.WeatherRecordTimestamp,
    //                                    WeatherStationId = (int)key.WeatherRecordWeatherStationId,
    //                                    SensorValues = sensorValues.Where(x => x.SensorValueId == key.WeatherRecordSensorValueId).Select(x => x.SensorValue).Cast < ISensorValue>().ToList()
    //                                }
    //                        }).ToList();



    //        var stations =
    //            mappedReader.GroupBy(x => new { x.WeatherStationId, x.Manufacturer, x.Model }, x => x,
    //                (key, g) =>
    //                    new WeatherStation
    //                    {
    //                        WeatherStationId = key.WeatherStationId,
    //                        Manufacturer = key.Manufacturer,
    //                        Model = key.Model,
    //                        Sensors = attachedSensors.Where(x => x.WeatherStationId == key.WeatherStationId).Select(y => y.Sensor).Cast<ISensor>().ToList(),
    //                        WeatherRecords = records.Where(x => x.WeatherRecord.WeatherStationId == key.WeatherStationId).Select(y => y.WeatherRecord).Cast<IWeatherRecord>().ToList()


    //                    }).Cast<IWeatherStation>().ToList();
            return null;
        }




        public async Task<List<IWeatherRecord>> GetWeatherRecordsForStationAsync(IWeatherStation station)
        {







            //var records = new List<WeatherRecord>();
            //using (
            //    var connection = new SQLiteConnection
            //    {
            //        ConnectionString =
            //            "Data Source=weather.sqlite;Version=3;foreign keys=true;datetimeformat=CurrentCulture;"
            //    })
            //{
            //    connection.Open();
            //    var sql = "SELECT * FROM WeatherRecords";
            //    using (var command = new SQLiteCommand(sql, connection))
            //    {
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

            return null;
        }

        public List<ISensorValue> GetSensorValuesForRecordId(int id)
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

        public async Task<int> AddStationAsync(IWeatherStation station)
        {
            var sql = "INSERT INTO WeatherStations (Manufacturer, Model, Latitude, Longitude) VALUES (@manufacturer, @model, @latitude, @longitude)";
            var sql2 = "SELECT last_insert_rowid();";
            SQLiteTransaction transaction = null;
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        transaction = connection.BeginTransaction();
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@manufacturer", station.Manufacturer);
                            command.Parameters.AddWithValue("@model", station.Model);
                            command.Parameters.AddWithValue("@latitude", station.Latitude);
                            command.Parameters.AddWithValue("@longitude", station.Longitude);
                            await command.ExecuteNonQueryAsync();

                            transaction.Commit();

                            var command2 = new SQLiteCommand(sql2, connection);
                            var id = command2.ExecuteScalar();
                            return (int)id;
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                transaction?.Rollback();
                _log.Error(ex.Message, ex);
                return 0;
            }
        }

        public async Task<List<ISensor>> GetSensorsForStationAsync(IWeatherStation station)
        {
            //using (
            //    var connection = new SQLiteConnection
            //    {
            //        ConnectionString = "Data Source=weather.sqlite;Version=3;foreign keys=true;"
            //    })
            //{
            //    connection.Open();

            //    var sensors = new List<ISensor>();
            //    var sql =
            //        "SELECT * FROM sensors LEFT JOIN unittypes ON sensors.unittype_id = unittypes.id WHERE station_id = @stationid";
            //    using (var command = new SQLiteCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@stationid", station.Id);
            //        var reader = command.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            var typ = reader[6].ToString();
            //            var pp = (Enums.UnitType)System.Enum.Parse(typeof(Enums.UnitType), typ);

            //            var sensor = new Sensor
            //            {
            //                Id = Convert.ToInt32(reader["id"]),
            //                Name = reader["name"].ToString(),
            //                Station = GetStationById(Convert.ToInt32(reader["station_id"])),
            //                Correction = Convert.ToDouble(reader["correction"].ToString()),
            //                Type = pp
            //            };
            //            sensors.Add(sensor);
            //        }
            //        return sensors;
            //    }
            //}
            return null;
        }

        public void CreateTables()
        {
        }

        public async Task<IWeatherStation> GetStationByIdAsync(int id)
        {
            var mappedReader = Enumerable.Empty<object>().Select(r => new
            {
                WeatherStationId = 0,
                Manufacturer = Empty,
                Model = Empty,
                Latitude = (double)0,
                Longitude = (double)0,
                SensorWeatherStationId = (int?)0,
                SensorId = (int?)0,
                SensorCorrection = 0,
                SensorName = Empty
            }).ToList();

            var sql = @"SELECT ws.[WeatherStationId] as WeatherStationId
                    , ws.[Manufacturer] as Manufacturer
		            ,ws.[Model] as Model
		            ,ws.[Latitude] as Latitude
		            ,ws.[Longitude] as Longitude
                    ,sr.[SensorId] as SensorId
		            ,sr.[Name] as SensorName
                    ,sr.[WeatherStationId] as SensorWeatherStationId
                    ,sr.[Correction] as SensorCorrection
                FROM [WeatherStations] ws
                   LEFT JOIN Sensors sr ON ws.WeatherStationId = sr.WeatherStationId WHERE ws.WeatherStationId = @id";
            using (var connection = new SQLiteConnection(DbConnectionString))
            {
                connection.Open();
                {
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (IDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                mappedReader.Add(new
                                {
                                    WeatherStationId = Convert.ToInt32(reader["WeatherStationId"]),
                                    Manufacturer = reader["Manufacturer"].ToString(),
                                    Model = reader["Model"].ToString(),
                                    Latitude = Convert.ToDouble(reader["Latitude"]),
                                    Longitude = Convert.ToDouble(reader["Longitude"]),
                                    SensorWeatherStationId = DbUtils.ParseIntNull(reader["SensorWeatherStationId"].ToString()),
                                    SensorId = DbUtils.ParseIntNull(reader["SensorId"].ToString()),
                                    SensorCorrection = DbUtils.ParseIntZero(reader["SensorCorrection"].ToString()),
                                    SensorName = reader["SensorName"].ToString(),
                                });
                            }
                        }
                    }
                }
            }

            // Sensors with a null SensorId are not attached to a station
            var attachedSensors =
                mappedReader.Where(r => r.SensorId != null)
                    .GroupBy(x => new { x.SensorId, x.SensorName, x.WeatherStationId, x.SensorCorrection }, x => x,
                        (key, g) =>
                            new
                            {
                                key.WeatherStationId,
                                Sensor =
                                    new Sensor
                                    {
                                        SensorId = (int)key.SensorId,
                                        Name = key.SensorName,
                                        Correction = key.SensorCorrection
                                    }
                            }).ToList();

            var station =
                mappedReader.GroupBy(x => new { x.WeatherStationId, x.Manufacturer, x.Model }, x => x,
                    (key, g) =>
                        new WeatherStation
                        {
                            WeatherStationId = key.WeatherStationId,
                            Manufacturer = key.Manufacturer,
                            Model = key.Model,
                            Sensors = attachedSensors.Where(x => x.WeatherStationId == key.WeatherStationId).Select(y => y.Sensor).Cast<ISensor>().ToList()
                        }).FirstOrDefault();
            return station;
        }

        public async Task DeleteStationAsync(IWeatherStation station)
        {
            var sql = "DELETE FROM WeatherStations WHERE WeatherStationId = @id";
            SQLiteTransaction transaction = null;
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        transaction = connection.BeginTransaction();
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@id", station.WeatherStationId);
                            await command.ExecuteNonQueryAsync();

                            transaction.Commit();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                transaction?.Rollback();
                _log.Error(ex.Message, ex);
            }
        }

        public async Task<int> UpdateStationAsync(IWeatherStation station)
        {
            var sql = "UPDATE WeatherStations SET Manufacturer=@manufacturer, Model=@model, Longitude=@longitude, Latitude=@latitude WHERE WeatherStationId = @id";
            SQLiteTransaction transaction = null;
            try
            {
                using (var connection = new SQLiteConnection(DbConnectionString))
                {
                    connection.Open();
                    {
                        transaction = connection.BeginTransaction();
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@manufacturer", station.Manufacturer);
                            command.Parameters.AddWithValue("@model", station.Model);
                            command.Parameters.AddWithValue("@longitude", station.Longitude);
                            command.Parameters.AddWithValue("@latitude", station.Latitude);
                            command.Parameters.AddWithValue("@id", station.WeatherStationId);
                            await command.ExecuteNonQueryAsync();

                            transaction.Commit();
                            return station.WeatherStationId;
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                transaction?.Rollback();
                _log.Error(ex.Message, ex);
                return 0;
            }
        }
    }
}