using System;
using System.Collections.Generic;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class Importer : IImporter
    {
        private readonly ISensorCore _sensorCore;
        private readonly IStationCore _stationCore;

        public Importer(ISensorCore sensorCore, IStationCore stationCore)
        {
            _sensorCore = sensorCore;
            _stationCore = stationCore;
        }

        public void Import(string filePath, WeatherStation station, List<Tuple<ISensor, int>> data,
            params int[] timestamp)
        {
            using (var csv = new CachedCsvReader(new StreamReader(filePath), false))
            {
                while (csv.ReadNextRecord())
                {
                    var dt = DateTime.Now;
                    if (timestamp.Length == 1)
                    {
                        dt = DateTime.Parse(csv[timestamp[0]]);
                    }
                    else if (timestamp.Length == 2)
                    {
                        dt = DateTime.Parse(csv[timestamp[0]] + " " + csv[timestamp[1]]);
                    }
                    var weatherrecord = new WeatherRecord {TimeStamp = dt, Station = station};

                    foreach (var d in data)
                    {
                        double value;
                        var s = new SensorValue();
                        if (double.TryParse(csv[d.Item2], out value))
                        {
                            s.Sensor = d.Item1 as Sensor;
                            s.RawValue = value;
                            if (s.Sensor.Type == Enums.UnitType.Temperature)
                            {
                                s.DisplayUnit = Units.Celsius;
                            }
                            if (s.Sensor.Type == Enums.UnitType.Humidity)
                            {
                                s.DisplayUnit = Units.Humidity;
                            }
                            if (s.Sensor.Type == Enums.UnitType.Pressure)
                            {
                                s.DisplayUnit = Units.Hectopascals;
                            }
                            if (s.Sensor.Type == Enums.UnitType.WindSpeed)
                            {
                                s.DisplayUnit = Units.Kmh;
                            }
                        }
                        _sensorCore.AddSensorValue(s);
                        weatherrecord.SensorValues.Add(s);
                    }
                    _stationCore.AddWeatherRecord(weatherrecord);
                    station.WeatherRecords.Add(weatherrecord);
                }
            }
        }
    }
}