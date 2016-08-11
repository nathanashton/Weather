using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LumenWorks.Framework.IO.Csv;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Entities;
using Weather.Common.EventArgs;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Helpers;
using static System.String;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ImportWindowViewModel : NotifyBase
    {
        private readonly IImporter _importer;
        private bool _multipleChecked;
        private bool _singleChecked;


        private Stopwatch s =
        new Stopwatch();

        public ImportWindowViewModel(IStationCore stationCore, IImporter importer)
        {
            Record = new ObservableCollection<Record>();
            Records = new ObservableCollection<ObservableCollection<Record>>();
            DateRecord = new ObservableCollection<Record>();
            DateRecords = new ObservableCollection<ObservableCollection<Record>>();
            FilteredRecords = new ObservableCollection<ObservableCollection<ViewModels.Record>>();
            FilteredDateRecords = new ObservableCollection<ObservableCollection<ViewModels.Record>>();

            _importer = importer;
            importer.ImportChanged += Importer_ImportChanged;
            importer.ImportComplete += Importer_ImportComplete;

            //TODO
          SelectedStation = stationCore.GetAllStations().GetAwaiter().GetResult()[0];

            Matches = new ObservableCollection<Match>();
            SingleChecked = true;
        }

        private void Importer_ImportComplete(object sender, EventArgs e)
        {
            s.Stop();
            var t = s.ElapsedMilliseconds;
            Importing = false;
        }

        public bool SingleChecked
        {
            get { return _singleChecked; }
            set
            {
                _singleChecked = value;
                OnPropertyChanged(() => SingleChecked);
                OnPropertyChanged(() => TimestampFormat);

                if (value)
                {
                    TimeFieldVisible = Visibility.Collapsed;
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
                OnPropertyChanged(() => TimestampFormat);

                if (value)
                {
                    TimeFieldVisible = Visibility.Visible;
                }
            }
        }

        public string TimestampFormat
        {
            get
            {
                if (SingleChecked)
                {
                    if (SelectedRecordDate != null)
                    {
                        try
                        {
                            var dt = DateTime.Parse(Format(SelectedRecordDate.Value));
                            return $"{dt:F}";
                        }
                        catch (FormatException)
                        {
                            return "Invalid Timestamp";
                        }
                    }
                }
                if (MultipleChecked)
                {
                    if (SelectedRecordDate != null && SelectedRecordTime != null)
                    {
                        try
                        {
                            var dt = DateTime.Parse(Format(SelectedRecordDate.Value + " " + SelectedRecordTime.Value));
                            return $"{dt:F}";
                        }
                        catch (FormatException)
                        {
                            return "Invalid Timestamp";
                        }
                    }
                }
                return "";
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
        public ObservableCollection<Match> Matches { get; set; }

        public ObservableCollection<ObservableCollection<Record>> FilteredRecords { get; set; }
        public ObservableCollection<ObservableCollection<Record>> FilteredDateRecords { get; set; }



        public Match SelectedMatch { get; set; }
        public Record SelectedRecordDate { get; set; }
        public Record SelectedRecordTime { get; set; }

        public Record SelectedRecord { get; set; }

        public Sensor SelectedSensor { get; set; }

        public ICommand MatchCommand
        {
            get
            {
                return new RelayCommand(Match,
                    x =>
                        SelectedSensor != null && SelectedRecord != null && !SensorAlreadyMatched(SelectedSensor) &&
                        !IndexAlreadyMatched(SelectedRecord.Index));
            }
        }

        public ICommand RemoveMatchCommand
        {
            get { return new RelayCommand(RemoveMatch, x => SelectedMatch != null); }
        }

        public bool Importing { get; set; }

        public ICommand ImportCommand
        {
            get { return new RelayCommand(Import, x => Matches.Count > 0 && TimeStampSelected() && !Importing); }
        }

        public int Progress { get; set; }

        public string FilePath { get; set; }

        public bool TimeStampSelected()
        {
            if (SingleChecked)
            {
                if (SelectedRecordDate != null)
                {
                    DateTime dt;
                    if (DateTime.TryParse(SelectedRecordDate.Value, out dt))
                    {
                        return true;
                    }
                }
            }
            if (MultipleChecked)
            {
                if (SelectedRecordDate != null && SelectedRecordTime != null)
                {
                    DateTime t;
                    if (DateTime.TryParse(SelectedRecordDate.Value + " " + SelectedRecordTime.Value, out t))
                    {
                        return true;
                    }
                }
            }
            return false;
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
                _importer.Import(FilePath, SelectedStation, data, ExcludeLineCount, SelectedRecordDate.Index);
            }
            if (MultipleChecked)
            {
                _importer.Import(FilePath, SelectedStation, data, ExcludeLineCount, SelectedRecordDate.Index, SelectedRecordTime.Index);
            }
            s.Start();
            _importer.Start();
            Importing = true;
        }

        private void Importer_ImportChanged(object sender, ImportEventArgs e)
        {
            if (e.Progress != null) Progress = (int) e.Progress;
        }

        public int ExcludeLineCount { get; set; }

        public void ReadFile(string filePath)
        {
            FilePath = filePath;
            DateRecord.Clear();
            DateRecords.Clear();
            Records.Clear();
            Record.Clear();

            FilteredDateRecords.Clear();
            FilteredRecords.Clear();

            using (var csv = new CachedCsvReader(new StreamReader(filePath), false))
            {
                while (csv.ReadNextRecord())
                {
                    var list = new ObservableCollection<Record>();
                    var dateList = new ObservableCollection<Record>();

                    for (var i = 0; i < csv.FieldCount; i++)
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
                    FilteredRecords.Add(list);
                    DateRecords.Add(dateList);
                    FilteredDateRecords.Add(dateList);
                }
            }
            RecordsCount = Records.Count;
            CurrentRecord = 1;
            FileSelected = true;

        }

        private bool CheckDate(string date)
        {
            var periods = date.Count(x => x == '.');
            var backslashed = date.Count(x => x == '\\');
            var forwardslashes = date.Count(x => x == '/');
            var colons = date.Count(x => x == ':');

            if (periods <= 1 && backslashed <= 1 && forwardslashes <= 1 && colons <= 0) return false;
            DateTime dt;
            return DateTime.TryParse(date, out dt);
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
            if (Matches.Any(match => match.Record.Index == index))
            {
                return true;
            }
            if (SelectedRecordDate?.Index == index) return true;
            return SelectedRecordTime?.Index == index;
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