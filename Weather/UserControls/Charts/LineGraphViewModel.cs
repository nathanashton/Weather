using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.UserControls.Charts
{
    [ImplementPropertyChanged]
    public class LineGraphViewModel : NotifyBase
    {
        private readonly IWeatherRecordCore _weatherRecordCore;
        private IStationSensor _selectedSensor;
        private int _sensorId;
       public string Header => "Line Graph";
        public ISelectedStation SelectedStation { get; set; }
        public ObservableCollection<GraphData> Data { get; set; }

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

        public LineGraphViewModel(ISelectedStation selectedStation, IWeatherRecordCore weatehrRecordCore)
        {
            _weatherRecordCore = weatehrRecordCore;
            SelectedStation = selectedStation;
        }

        public void SelectedStation_TimeSpanChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => Title);
            DrawGraph();
        }

        public void SelectedStation_SelectedStationsChanged(object sender, EventArgs e)
        {
            if ((SelectedStation != null) && (SelectedStation.WeatherStation != null))
            {
                SelectedSensor =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(x => x.StationSensorId == _sensorId);
            }
            DrawGraph();
        }

        public void DrawGraph()
        {
            if ((SelectedStation == null) || (SelectedStation.WeatherStation == null) || (SelectedSensor == null))
            {
                return;
            }


            //if (SelectedStation.WeatherStation.Records == null || SelectedStation.WeatherStation.Records.Count == 0)
            //{
            //    SelectedStation.WeatherStation.Records =
            //        new ObservableCollection<IWeatherRecord>(
            //            _weatherRecordCore.GetAllRecordsForStationBetweenDates(
            //                SelectedStation.WeatherStation.WeatherStationId,
            //                SelectedStation.StartDate, SelectedStation.EndDate));
            //}

            var records =
                SelectedStation.WeatherStation.Records.Where(
                        x => x.SensorValues.Any(r => r.Sensor.SensorType.Name == SelectedSensor.Sensor.SensorType.Name))
                    .ToList();

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
            }).ToList().OrderBy(x=> x.Date);


            Data = new ObservableCollection<GraphData>(data);
        }
    }


    [ImplementPropertyChanged]
    public class GraphData
    {
        public DateTime Date { get; set; }
        public double? Value { get; set; }
    }
}