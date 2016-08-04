using LumenWorks.Framework.IO.Csv;
using System;
using System.IO;
using System.Linq;
using Weather.Common.Entities;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class Importer : IImporter
    {
        private ISensorCore _sensorCore;
        private IStationCore _stationCore;

        public Importer(ISensorCore sensorCore, IStationCore stationCore)
        {
            _sensorCore = sensorCore;
            _stationCore = stationCore;
        }

        public void Import(string filePath, WeatherStation station)
        {
            var tempSensor = station.Sensors.ToList()[0];
            var humsensor = station.Sensors.ToList()[1];

            using (var csv = new CachedCsvReader(new StreamReader(filePath), false))
            {
                while (csv.ReadNextRecord())
                {
                    var dt = DateTime.Parse(csv[0] + " " + csv[1]);

                    double temp;
                    var temperature = new SensorValue();
                    if (double.TryParse(csv[2], out temp))
                    {
                        temperature.Sensor = tempSensor;
                        temperature.RawValue = temp;
                        temperature.DisplayUnit = Units.Celsius;
                    }

                    double hum;
                    var humidity = new SensorValue();
                    if (double.TryParse(csv[3], out hum))
                    {
                        humidity.Sensor = humsensor;
                        humidity.RawValue = hum;
                        humidity.DisplayUnit = Units.Hectopascals;
                    }

                    _sensorCore.AddSensorValue(temperature);
                    _sensorCore.AddSensorValue(humidity);

                    var weatherrecord = new WeatherRecord { TimeStamp = dt, Station = station };

                    weatherrecord.SensorValues.Add(temperature);
                    weatherrecord.SensorValues.Add(humidity);
                    _stationCore.AddWeatherRecord(weatherrecord);

                    station.WeatherRecords.Add(weatherrecord);
                }
            }
        }
    }
}