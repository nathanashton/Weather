using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Interfaces;

namespace Weather.Charts.LineGraph
{
    [ImplementPropertyChanged]
    public class LineGraphControlViewModel : NotifyBase, IChartViewModel
    {
        private IStationSensor _selectedSensor;
        private IStationSensor _selectedSensor2;

        private long _sensorId;
        private long _sensorId2;

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
                else
                {
                    _sensorId = 0;
                }
                _selectedSensor = value;
                DrawChart();
                OnPropertyChanged(() => SelectedSensor);
                OnPropertyChanged(() => Title);
            }
        }

        public IStationSensor SelectedSensor2
        {
            get { return _selectedSensor2; }
            set
            {
                _sensorId2 = value?.StationSensorId ?? 0;
                _selectedSensor2 = value;
                DrawChart();
                OnPropertyChanged(() => SelectedSensor2);
                OnPropertyChanged(() => Title);
            }
        }

        public string Title
        {
            get
            {
                if ((SelectedSensor != null) && (SelectedSensor2 == null))
                {
                    return SelectedSensor.Sensor.SensorType.ToString();
                }
                if ((SelectedSensor != null) && (SelectedSensor2 != null))
                {
                    return SelectedSensor.Sensor.SensorType + " / " + SelectedSensor2.Sensor.SensorType;
                }
                if ((SelectedSensor == null) && (SelectedSensor2 != null))
                {
                    return SelectedSensor2.Sensor.ToString();
                }
                return "Line Graph";
            }
        }

        public LineGraphControlViewModel(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
        }

        //private System.Threading.Tasks.Task SelectedStation_RecordsUpdated(object sender, EventArgs e)
        //{
           
        //}

        public string Header => "Line Graph";


        public ISelectedStation SelectedStation { get; set; }


        public Task SelectedStation_RecordsUpdated(object sender, EventArgs e)
        {
            if (SelectedStation?.WeatherStation != null)
            {
                SelectedSensor =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(x => x.StationSensorId == _sensorId);
                SelectedSensor2 =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(x => x.StationSensorId == _sensorId2);
            }
            OnPropertyChanged(() => Title);
            DrawChart();
            return null;
        }

        public void SelectedStation_GetRecordsCompleted(object sender, EventArgs e)
        {
            DrawChart();
        }

        public void SelectedStation_SelectedStationChanged(object sender, EventArgs e)
        {
            //
        }

        public void DrawChart()
        {
            if ((SelectedStation?.WeatherStation == null) && ((SelectedSensor == null) || (SelectedSensor2 == null)))
            {
                Data = null;
                Data2 = null;

                return;
            }

            Data = null;
            Data2 = null;

            if (SelectedSensor != null)
            {
                var sensorValues = new List<T>();

                foreach (var record in SelectedStation.WeatherStation.Records)
                {
                    foreach (var s in record.SensorValues)
                    {
                        if (s.Sensor.SensorType.SensorTypeId == SelectedSensor.Sensor.SensorType.SensorTypeId)
                        {
                            var f = new T
                            {
                                TimeStamp = record.TimeStamp,
                                SensorValue = s
                            };
                            sensorValues.Add(f);
                        }
                    }
                }


                var data = sensorValues.GroupBy(x => new {x.TimeStamp, x.SensorValue}, x => x, (key, g) => new GraphData
                {
                    Date = key.TimeStamp,
                    Value = key.SensorValue.CorrectedValue
                }).ToList().OrderBy(x => x.Date);


                Data = new ObservableCollection<GraphData>(data);
            }

            if (SelectedSensor2 != null)
            {
                var sensorValues = new List<T>();

                foreach (var record in SelectedStation.WeatherStation.Records)
                {
                    foreach (var s in record.SensorValues)
                    {
                        if (s.Sensor.SensorType.SensorTypeId == SelectedSensor2.Sensor.SensorType.SensorTypeId)
                        {
                            var f = new T
                            {
                                TimeStamp = record.TimeStamp,
                                SensorValue = s
                            };
                            sensorValues.Add(f);
                        }
                    }
                }


                var data2 = sensorValues.GroupBy(x => new {x.TimeStamp, x.SensorValue}, x => x,
                    (key, g) => new GraphData
                    {
                        Date = key.TimeStamp,
                        Value = key.SensorValue.CorrectedValue
                    }).ToList().OrderBy(x => x.Date);


                Data2 = new ObservableCollection<GraphData>(data2);
            }
        }
    }


    [ImplementPropertyChanged]
    public class GraphData
    {
        public DateTime Date { get; set; }
        public double? Value { get; set; }
    }

    [ImplementPropertyChanged]
    public class T
    {
        public DateTime TimeStamp { get; set; }
        public ISensorValue SensorValue { get; set; }
    }
}