using LumenWorks.Framework.IO.Csv;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Weather.Common.Entities;
using Weather.Common.Entities.SensorValues;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public ObservableCollection<WeatherStation> Stations { get; set; }
        private readonly ILog _log;
        private readonly IStationCore _stationCore;
        private readonly ISensorCore _sensorCore;

        public WeatherStation SelectedStation { get; set; }


        public MainWindowViewModel(ILog log, IStationCore stationCore, ISensorCore sensorCode)
        {
            _stationCore = stationCore;
            _sensorCore = sensorCode;
            _stationCore.StationsChanged += _stationCore_StationsChanged;
            Station = new WeatherStation
            {
                Model = "LW301",
                Manufacturer = "Oregon"
            };
          _stationCore.GetAllStations();
           Stations = _stationCore.Stations;
        }

        private void _stationCore_StationsChanged(object sender, EventArgs e)
        {
            Stations = _stationCore.Stations;
        }

        public void Add(WeatherStation station)
        {
            _stationCore.AddStation(station);
        }

        public IWeatherStation Station { get; set; }

        public void Go()
        {
            //_stationCore.CreateTables();
  //LoadCsvFirst();
          //  LoadCsvSecond();

            //  var t = Station.GetValuesForSensorType(Enums.UnitType.Pressure);
            //var allstation = _stationCore.GetAllStations();
            //var all = _stationCore.GetRecordsForStation(allstation.First());
        }

        public void LoadCsvSecond()
        {
            var stationone = Stations[1];

            var tempSensor = stationone.Sensors.First(x => x.Type == Enums.UnitType.Temperature);
          

            var filePath = @"C:\Users\nathana\Desktop\temp2.csv";
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
                    record.SensorValues.Add(temperature);
                    record.Station = stationone;
                    stationone.AddRecord(record);
                }
            }
        }

        public void LoadCsvFirst()
        {
            var stationone = Stations[0];

            var tempSensor = stationone.Sensors.First(x => x.Type == Enums.UnitType.Temperature);
            var humiditySensor = stationone.Sensors.First(x => x.Type == Enums.UnitType.Humidity);
            var pressureSensor = stationone.Sensors.First(x => x.Type == Enums.UnitType.Pressure);
          //  var windspeedSensor = stationone.Sensors.First(x => x.Type == Enums.UnitType.WindSpeed);

            var filePath = @"C:\Users\nathana\Desktop\temp2.csv";
            using (var csv = new CachedCsvReader(new StreamReader(filePath), false))
            {
                while (csv.ReadNextRecord())
                {
                    var dt = DateTime.Parse(csv[0] + " " + csv[1]);
                    var record = new WeatherRecord { TimeStamp = dt };

                    double temp;
                    Temperature temperature;
                    if (double.TryParse(csv[2], out temp))
                    {
                        temperature = new Temperature(temp) { Sensor = tempSensor };
                    }
                    else
                    {
                        temperature = new Temperature(null) { Sensor = tempSensor };
                    }

                    double hum;
                    Humidity humidity;
                    if (double.TryParse(csv[3], out hum))
                    {
                        humidity = new Humidity(hum) { Sensor = humiditySensor };
                    }
                    else
                    {
                        humidity = new Humidity(null) { Sensor = humiditySensor };
                    }

                    double pres;
                    Pressure pressure;
                    if (double.TryParse(csv[4], out pres))
                    {
                        pressure = new Pressure(pres) { Sensor = pressureSensor };
                    }
                    else
                    {
                        pressure = new Pressure(null) { Sensor = pressureSensor };
                    }

                    //double wind;
                    //WindSpeed windspeed;
                    //var pp = csv[5];
                    //if (double.TryParse(csv[6], out wind))
                    //{
                    //    windspeed = new WindSpeed(wind) { Sensor = windspeedSensor };
                    //}
                    //else
                    //{
                    //    windspeed = new WindSpeed(null) { Sensor = windspeedSensor };
                    //}

                    //Save Sensor values
                    //_sensorCore.AddSensorValue(temperature);
                    //_sensorCore.AddSensorValue(humidity);
                    //_sensorCore.AddSensorValue(pressure);
                    //_sensorCore.AddSensorValue(windspeed);



                    record.SensorValues.Add(temperature);
                    record.SensorValues.Add(humidity);
                    record.SensorValues.Add(pressure);
                 //   record.SensorValues.Add(windspeed);
                    record.Station = stationone;

                    ////save record
                    stationone.AddRecord(record);
                }
            }




            foreach (var sensor in stationone.WeatherRecords)
            {
                _sensorCore.AddSensorValues(sensor.SensorValues);
            }


            _sensorCore.AddWeatherRecords(stationone.WeatherRecords);
          


         


        }
    }
}