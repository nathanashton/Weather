using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.UserControls.Charts
{
    [ImplementPropertyChanged]
    public class LineGraphViewModel : NotifyBase
    {
        private IStationSensor _selectedSensor;
        private IStationSensor _selectedSensor2;

        private int _sensorId;
        private int _sensorId2;

        public string Header => "Line Graph";
        public ISelectedStation SelectedStation { get; set; }
        public ObservableCollection<GraphData> Data { get; set; }
        public ObservableCollection<GraphData> Data2 { get; set; }


        public IStationSensor SelectedSensor
        {
            get { return _selectedSensor; }
            set
            {
                if (value != null)
                {
                    _sensorId = value.StationSensorId;
                }
                _selectedSensor = value;
                DrawGraph();
                OnPropertyChanged(() => SelectedSensor);
            }
        }

        public IStationSensor SelectedSensor2
        {
            get { return _selectedSensor2; }
            set
            {
                if (value != null)
                {
                    _sensorId2 = value.StationSensorId;
                }
                _selectedSensor2 = value;
                DrawGraph();
                OnPropertyChanged(() => SelectedSensor2);
            }
        }

        public string Title
        {
            get
            {
                if (SelectedSensor == null)
                {
                    return "Line Graph (" + SelectedStation.TimeSpanWords + ")";
                }
                return "Line Graph " + SelectedSensor.Sensor.SensorType + " (" + SelectedStation.TimeSpanWords + ")";
            }
        }

        private readonly ILog _log;

        public LineGraphViewModel(ISelectedStation selectedStation, ILog log)
        {
            _log = log;
            SelectedStation = selectedStation;
        }

        public void SelectedStation_GetRecordsCompleted(object sender, EventArgs e)
        {
            MessageBox.Show("Complete");
            DrawGraph();
        }

        public void SelectedStation_TimeSpanChanged(object sender, EventArgs e)
        {
            _log.Debug("Time span changed");
            OnPropertyChanged(() => Title);
            DrawGraph();
        }

        public void SelectedStation_SelectedStationsChanged(object sender, EventArgs e)
        {
            if (SelectedStation?.WeatherStation != null)
            {
                SelectedSensor =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(x => x.StationSensorId == _sensorId);
                SelectedSensor2 =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(x => x.StationSensorId == _sensorId2);
            }
            DrawGraph();
        }

        public void DrawGraph()
        {
            _log.Debug("Draw Graph");
            if (SelectedStation?.WeatherStation == null || (SelectedSensor == null))
            {
                Data = null;
                Data2 = null;
                _log.Debug("Null - do not Draw Graph");

                return;
            }

            Data = null;
            Data2 = null;

            var records = SelectedStation.WeatherStation.Records.Where(x => x.SensorValues.Any(r => r.Sensor.SensorType.Name == SelectedSensor.Sensor.SensorType.Name)).ToList();
            _log.Debug(SelectedStation.WeatherStation.Records.Count.ToString());
            
            var sensorValues = new List<T>();
            foreach (var record in records)
            {
                foreach (var r in record.SensorValues)
                {
                    var f = new T
                    {
                        TimeStamp = record.TimeStamp,
                        SensorValue = r
                    };
                    sensorValues.Add(f);
                }
            }

            var data = sensorValues.GroupBy(x => new {x.TimeStamp, x.SensorValue}, x => x, (key, g) => new GraphData
            {
                Date = key.TimeStamp,
                Value = key.SensorValue.CorrectedValue
            }).ToList().OrderBy(x => x.Date);


            Data = new ObservableCollection<GraphData>(data);

            // Sensor 2
            if (SelectedSensor2 == null)
            {
                return;
            }

            var records2 =
                SelectedStation.WeatherStation.Records.Where(
                        x => x.SensorValues.Any(r => r.Sensor.SensorType.Name == SelectedSensor2.Sensor.SensorType.Name))
                    .ToList();

            var sensorValues2 = new List<T>();
            foreach (var record in records2)
            {
                foreach (var r in record.SensorValues)
                {
                    var f = new T
                    {
                        TimeStamp = record.TimeStamp,
                        SensorValue = r
                    };
                    sensorValues2.Add(f);
                }
            }

            var data2 = sensorValues2.GroupBy(x => new {x.TimeStamp, x.SensorValue}, x => x, (key, g) => new GraphData
            {
                Date = key.TimeStamp,
                Value = key.SensorValue.CorrectedValue
            }).ToList().OrderBy(x => x.Date);
            Data2 = new ObservableCollection<GraphData>(data2);
        }
    }


    [ImplementPropertyChanged]
    public class GraphData
    {
        public DateTime Date { get; set; }
        public double? Value { get; set; }
    }
}