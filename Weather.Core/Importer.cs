using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Weather.Common.Entities;
using Weather.Common.EventArgs;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class Importer : IImporter
    {
        private readonly ISensorCore _sensorCore;
        private readonly IStationCore _stationCore;
        private List<Tuple<ISensor, int>> _data;
        private int _excludeLines;
        private string _filePath;
        private WeatherStation _station;
        private int[] _timestamp;
        private readonly BackgroundWorker _worker;

        public Importer(ISensorCore sensorCore, IStationCore stationCore)
        {
            _sensorCore = sensorCore;
            _stationCore = stationCore;
            _worker = new BackgroundWorker();
            _worker.DoWork += _worker_DoWork;
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.WorkerReportsProgress = true;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
        }

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        public event EventHandler<ImportEventArgs> ImportChanged;
        public event EventHandler ImportComplete;

        private void OnChanged(int p)
        {
            ImportChanged?.Invoke(this, new ImportEventArgs {Progress = p});
        }

        public void Import(string filePath, WeatherStation station, List<Tuple<ISensor, int>> data, int excludeLines,
            params int[] timestamp)
        {
            _filePath = filePath;
            _data = data;
            _timestamp = timestamp;
            _station = station;
            _excludeLines = excludeLines;
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ImportComplete?.Invoke(this,null);
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnChanged(e.ProgressPercentage);
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var lineCount = File.ReadLines(_filePath).Count();
            using (var csv = new CachedCsvReader(new StreamReader(_filePath), false))
            {
                while (csv.ReadNextRecord())
                {
                    if (csv.CurrentRecordIndex >= _excludeLines)
                    {

                        var dt = DateTime.Now;
                        if (_timestamp.Length == 1)
                        {
                            dt = DateTime.Parse(csv[_timestamp[0]]);
                        }
                        else if (_timestamp.Length == 2)
                        {
                            dt = DateTime.Parse(csv[_timestamp[0]] + " " + csv[_timestamp[1]]);
                        }
                        var weatherrecord = new WeatherRecord {TimeStamp = dt, Station = _station};

                        foreach (var d in _data)
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

                        var pr = Utils.CalculatePercentage(csv.CurrentRecordIndex + 1, 0, lineCount);
                        _worker.ReportProgress(pr);
                    }
                }
            }
        }
    }
}