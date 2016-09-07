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
    public class MinMaxViewModel : NotifyBase
    {
        private readonly IWeatherRecordCore _weatherRecordCore;
        private IStationSensor _selectedSensor;
        public string Header => "Minimum / Maximum";
        public ISelectedStation SelectedStation { get; set; }
        private int _sensorId;
        public ObservableCollection<HiLo> Data { get; set; }

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
                    return "Min / Max (" + SelectedStation.TimeSpanWords + ")";
                }
                return "Min / Max " + SelectedSensor.Sensor.SensorType + " (" + SelectedStation.TimeSpanWords + ")";
            }
        }

        public MinMaxViewModel(ISelectedStation selectedStation, IWeatherRecordCore weatehrRecordCore)
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
            if (SelectedStation != null && SelectedStation.WeatherStation != null)
            {
                SelectedSensor = SelectedStation.WeatherStation.Sensors.FirstOrDefault(x => x.StationSensorId == _sensorId);
            }
            DrawGraph();
        }

        public void DrawGraph()
        {
            if ((SelectedStation == null) || (SelectedStation.WeatherStation == null) || (SelectedSensor == null))
            {
                return;
            }

            var tyt = SelectedStation.StartDate;
            var fyt = SelectedStation.EndDate;


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

            var data =
                sensorValues.Select(t => new {t, dt = t.TimeStamp}).GroupBy(
                        t1 => new {y = t1.dt.Year, m = t1.dt.Month, d = t1.dt.Day}, t1 => t1.t)
                    .Select(
                        dtd => new HiLo
                        {
                            Date = new DateTime(dtd.Key.y, dtd.Key.m, dtd.Key.d),
                            Min = (double) dtd.Min(x => x.SensorValue.CorrectedValue),
                            Max = (double) dtd.Max(x => x.SensorValue.CorrectedValue)
                        }).ToList();

            Data = new ObservableCollection<HiLo>(data);
        }
    }

    [ImplementPropertyChanged]
    public class HiLo
    {
        public DateTime Date { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }

    [ImplementPropertyChanged]
    public class T
    {
        public DateTime TimeStamp { get; set; }
        public ISensorValue SensorValue { get; set; }
    }
}