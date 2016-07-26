using System;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Entities.SensorValues;
using Weather.Common.Interfaces;
using Weather.Core;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        private readonly ILog _log;
        public IWeatherStation Station { get; set; }

        public MainWindowViewModel(ILog log)
        {
            Station = new WeatherStation
            {
                Model = "LW301",
                Manufacturer = "Oregon"
            };

            var core = new Test();
            core.go();

        }

        public void Go()
        {
  
            LoadCsv();
            var t = Station.GetValuesForSensorType(Enums.UnitType.Pressure);
        }

        public void LoadCsv()
        {
            var tempSensor = new Sensor
            {
                Name = "Outdoor Temperature",
                Type = Enums.UnitType.Temperature
            };

            var humiditySensor = new Sensor
            {
                Name = "Humidity",
                Type = Enums.UnitType.Humidity
            };

            var pressureSensor = new Sensor
            {
                Name = "Pressure",
                Type = Enums.UnitType.Pressure
            };

            var windspeedSensor = new Sensor
            {
                Name = "Wind Average",
                Type = Enums.UnitType.WindSpeed
            };


            for (int i = 1; i < 2; i++)
            {


                var filePath = @"C:\Users\nathana\Desktop\temp.csv";
                using (var csv = new CachedCsvReader(new StreamReader(filePath), false))
                {
                    while (csv.ReadNextRecord())
                    {
                        var dt = DateTime.Parse(csv[0] + " " + csv[1]);
                        var record = new WeatherRecord {TimeStamp = dt};

                        double temp;
                        Temperature temperature;
                        if (double.TryParse(csv[2], out temp))
                        {
                            temperature = new Temperature(temp) {Sensor = tempSensor};
                        }
                        else
                        {
                            temperature = new Temperature(null) {Sensor = tempSensor};
                        }

                        double hum;
                        Humidity humidity;
                        if (double.TryParse(csv[3], out hum))
                        {
                            humidity = new Humidity(hum) {Sensor = humiditySensor};
                        }
                        else
                        {
                            humidity = new Humidity(null) {Sensor = humiditySensor};
                        }

                        double pres;
                        Pressure pressure;
                        if (double.TryParse(csv[4], out pres))
                        {
                            pressure = new Pressure(pres) {Sensor = pressureSensor};
                        }
                        else
                        {
                            pressure = new Pressure(null) {Sensor = pressureSensor};
                        }

                        double wind;
                        WindSpeed windspeed;
                        var pp = csv[5];
                        if (double.TryParse(csv[6], out wind))
                        {
                            windspeed = new WindSpeed(wind) {Sensor = windspeedSensor};
                        }
                        else
                        {
                            windspeed = new WindSpeed(null) {Sensor = windspeedSensor};
                        }

                        record.SensorValues.Add(temperature);
                        record.SensorValues.Add(humidity);
                        record.SensorValues.Add(pressure);
                        record.SensorValues.Add(windspeed);

                        Station.AddRecord(record);
                    }
                }
            }
        }
    }
}