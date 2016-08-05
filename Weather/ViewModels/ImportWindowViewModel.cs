using LumenWorks.Framework.IO.Csv;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Weather.Common.Entities;
using Weather.Core.Interfaces;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ImportWindowViewModel
    {
        public int CurrentRecord { get; set; }
        public string CurrentRecordDisplay => "of " + RecordsCount;
        public WeatherStation SelectedStation { get; set; }
        public int RecordsCount { get; set; }
        public ObservableCollection<string> Record { get; set; }
        public ObservableCollection<ObservableCollection<string>> Records { get; set; }
        private IStationCore _stationCore;

        public ImportWindowViewModel(IStationCore stationCore)
        {
            Record = new ObservableCollection<string>();
            Records = new ObservableCollection<ObservableCollection<string>>();
            _stationCore = stationCore;
            SelectedStation = _stationCore.Stations.First();
        }

        public void ReadFile(string filePath)
        {
            Records.Clear();
            Record.Clear();
            using (var csv = new CachedCsvReader(new StreamReader(filePath), false))
            {
                while (csv.ReadNextRecord())
                {
                    var list = new ObservableCollection<string>();
                    for (int i = 0; i < csv.FieldCount; i++)
                    {
                        list.Add(csv[i]);
                    }
                    Records.Add(list);
                }
            }
            RecordsCount = Records.Count;
            CurrentRecord = 1;
        }
    }
}