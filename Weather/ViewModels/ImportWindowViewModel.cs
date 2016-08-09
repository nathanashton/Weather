using LumenWorks.Framework.IO.Csv;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Weather.Common;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ImportWindowViewModel : NotifyBase
    {
        private bool _singleChecked;
        private bool _multipleChecked;

        public bool SingleChecked
        {
            get { return _singleChecked; }
            set
            {
                _singleChecked = value;
                OnPropertyChanged(() => SingleChecked);
                if (value)
                {
                    TimeFieldVisible = Visibility.Hidden;
                }
            }
        }

        public bool MultipleChecked
        {
            get { return _multipleChecked; }
            set
            {
                _multipleChecked = value;
                OnPropertyChanged(() => MultipleChecked);
                if (value)
                {
                    TimeFieldVisible = Visibility.Visible;
                }
            }
        }
        public bool FileSelected { get; set; }
        public ObservableCollection<Record> DateRecord { get; set; }
        public ObservableCollection<ObservableCollection<Record>> DateRecords { get; set; }

        public Visibility TimeFieldVisible { get; set; }
        public int SelectedRecordIndex { get; set; }
        public int CurrentRecord { get; set; }
        public string CurrentRecordDisplay => "of " + RecordsCount;
        public WeatherStation SelectedStation { get; set; }
        public int RecordsCount { get; set; }
        public ObservableCollection<Record> Record { get; set; }
        public ObservableCollection<ObservableCollection<Record>> Records { get; set; }
        private IStationCore _stationCore;
        public ObservableCollection<Match> Matches { get; set; }
        public Match SelectedMatch { get; set; }
        private IImporter _importer;
        public Record SelectedRecordDate { get; set; }
        public Record SelectedRecordTime { get; set; }

        public Record SelectedRecord { get; set; }

        public Sensor SelectedSensor { get; set; }

        public ICommand MatchCommand
        {
            get { return new RelayCommand(Match, x => SelectedSensor != null && SelectedRecord != null && !SensorAlreadyMatched(SelectedSensor) && !IndexAlreadyMatched(SelectedRecord.Index)); }
        }

        public ICommand RemoveMatchCommand
        {
            get { return new RelayCommand(RemoveMatch, x => SelectedMatch != null); }
        }

        public ICommand ImportCommand
        {
            get { return new RelayCommand(Import, x => Matches.Count > 0); }
        }

        private void Import(object obj)
        {
            var data = new List<Tuple<ISensor, int>>();
            foreach (var match in Matches)
            {
                var t = new Tuple<ISensor, int>(match.Sensor, match.Record.Index);
                data.Add(t);
            }

            if (SingleChecked)
            {
                _importer.Import(FilePath, SelectedStation, data, SelectedRecordDate.Index);
            }
            if (MultipleChecked)
            {
                _importer.Import(FilePath, SelectedStation, data, SelectedRecordDate.Index, SelectedRecordTime.Index);
            }
            _importer.Start();
        }



        public int Progress { get; set; }

        public string FilePath { get; set; }

        public ImportWindowViewModel(IStationCore stationCore, IImporter importer)
        {
            Record = new ObservableCollection<Record>();
            Records = new ObservableCollection<ObservableCollection<Record>>();
            DateRecord = new ObservableCollection<ViewModels.Record>();
            DateRecords = new ObservableCollection<ObservableCollection<Record>>();
            _importer = importer;
            _stationCore = stationCore;
            importer.ImportChanged += Importer_ImportChanged;


            //TODO
            SelectedStation = _stationCore.GetAllStations()[0];


            Matches = new ObservableCollection<ViewModels.Match>();
            SingleChecked = true;
        }

        private void Importer_ImportChanged(object sender, Common.EventArgs.ImportEventArgs e)
        {
            Progress = (int)e.Progress;

        }

        public void ReadFile(string filePath)
        {
            FilePath = filePath;
            DateRecord.Clear();
            DateRecords.Clear();
            Records.Clear();
            Record.Clear();
            using (var csv = new CachedCsvReader(new StreamReader(filePath), false))
            {
                while (csv.ReadNextRecord())
                {
                    var list = new ObservableCollection<Record>();
                    var dateList = new ObservableCollection<Record>();

                    for (int i = 0; i < csv.FieldCount; i++)
                    {
                        var v = csv[i];

                        if (!CheckDate(v))
                        {
                            var r = new Record
                            {
                                Value = v,
                                Index = i
                            };
                            list.Add(r);
                        }
                        else
                        {
                            var r = new Record
                            {
                                Value = v,
                                Index = i
                            };

                            dateList.Add(r);
                        }
                    }
                    Records.Add(list);
                    DateRecords.Add(dateList);
                }
            }
            RecordsCount = Records.Count;
            CurrentRecord = 1;
            FileSelected = true;
        }

        private bool CheckDate(String date)
        {
            var periods = date.Count(x => x == '.');
            var backslashed = date.Count(x => x == '\\');
            var forwardslashes = date.Count(x => x == '/');
            var colons = date.Count(x => x == ':');

            if (periods > 1 || backslashed > 1 || forwardslashes > 1 || colons > 0)
            {
                DateTime dt;
                return DateTime.TryParse(date, out dt);
            }
            return false;
        }

        private void Match(object obj)
        {
            var match = new Match
            {
                Sensor = SelectedSensor,
                Record = SelectedRecord
            };
            Matches.Add(match);
            SelectedRecord = null;
            SelectedSensor = null;
        }

        private void RemoveMatch(object obj)
        {
            Matches.Remove(SelectedMatch);
            SelectedRecord = null;
            SelectedSensor = null;
        }

        private bool SensorAlreadyMatched(Sensor sensor)
        {
            return Matches.Any(x => x.Sensor == sensor);
        }

        private bool IndexAlreadyMatched(int index)
        {
            foreach (var match in Matches)
            {
                if (match.Record.Index == index) return true;
            }
            if (SelectedRecordDate?.Index == index) return true;
            if (SelectedRecordTime?.Index == index) return true;
            return false;
        }
    }

    [ImplementPropertyChanged]
    public class Match
    {
        public Sensor Sensor { get; set; }
        public Record Record { get; set; }

        public override string ToString()
        {
            return Sensor.Name + " => " + Record.Value + " (" + Record.Index + ")";
        }
    }

    [ImplementPropertyChanged]
    public class Record
    {
        public int Index { get; set; }
        public string Value { get; set; }
    }
}