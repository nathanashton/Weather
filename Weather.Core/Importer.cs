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
        private readonly IWeatherRecordCore _weatherRecordCore;
        private readonly BackgroundWorker _worker;
        private ISensorValueCore _sensorValueCore;

        private readonly List<SensorValue> listSensorValues = new List<SensorValue>();
        private readonly List<WeatherRecord> listWeatherRecords = new List<WeatherRecord>();
        private List<Tuple<ISensor, int>> _data;
        private int _excludeLines;
        private string _filePath;
        private IWeatherStation _station;
        private int[] _timestamp;

        public Importer(ISensorCore sensorCore, IWeatherRecordCore weatherRecordCore, ISensorValueCore sensorValueCore)
        {
            _sensorValueCore = sensorValueCore;
            _sensorCore = sensorCore;
            _weatherRecordCore = weatherRecordCore;
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

        public void Import(string filePath, IWeatherStation station, List<Tuple<ISensor, int>> data, int excludeLines,
            params int[] timestamp)
        {
            _filePath = filePath;
            _data = data;
            _timestamp = timestamp;
            _station = station;
            _excludeLines = excludeLines;
        }

        private void OnChanged(int p)
        {
            ImportChanged?.Invoke(this, new ImportEventArgs {Progress = p});
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //_sensorCore.AddSensorValues(listSensorValues);
            //   _stationCore.AddWeatherRecords(listWeatherRecords);
            ImportComplete?.Invoke(this, null);
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnChanged(e.ProgressPercentage);
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //   var listSensorValues = new List<SensorValue>();
            // var listWeatherRecords = new List<WeatherRecord>();

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
                        //    var weatherrecord = new WeatherRecord {TimeStamp = dt, Station = _station};
                        var weatherrecord = new WeatherRecord {TimeStamp = dt, SensorValues = new List<ISensorValue>()};

                        foreach (var d in _data)
                        {
                            double value;
                            var s = new SensorValue();
                            if (double.TryParse(csv[d.Item2], out value))
                            {
                                s.Sensor = d.Item1 as ISensor;
                                s.RawValue = value;
                            }


                            // Add SensorValue to DB
                         //   s.SensorValueId =_sensorValueCore.Add(s).SensorValueId;


                            weatherrecord.SensorValues.Add(s);
                            weatherrecord.WeatherStationId = _station.WeatherStationId;
                        }

                        // Add Weather Refo
                        weatherrecord.WeatherRecordId = _weatherRecordCore.Add(weatherrecord);

                        foreach (var v in weatherrecord.SensorValues)
                        {
                            v.SensorValueId = _sensorValueCore.Add(v).SensorValueId;

                            // Add record to WeatherRecords_SensorValues
                            _weatherRecordCore.AddWeatherRecordSensorValue(weatherrecord.WeatherRecordId,
                                v.SensorValueId);
                        }

                        var pr = Utils.CalculatePercentage(csv.CurrentRecordIndex + 1, 0, lineCount);
                        _worker.ReportProgress(pr);
                    }
                }
            }
        }
    }
}