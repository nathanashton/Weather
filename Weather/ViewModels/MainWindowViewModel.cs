using LumenWorks.Framework.IO.Csv;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Weather.Common.Entities;
using Weather.Core;
using Weather.Core.Interfaces;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public ObservableCollection<WeatherStation> Stations { get; set; }
        private readonly IStationCore _stationCore;
        private readonly IImporter _importer;
        private readonly Database _database;

        public WeatherStation SelectedStation { get; set; }

        public MainWindowViewModel(IStationCore stationCore, IImporter importer, Database database)
        {
            _stationCore = stationCore;
            _importer = importer;
            _database = database;
            Stations = _stationCore.Stations;
        }

        public void ImportRecords()
        {
            _importer.Import(@"C:\Users\nathana\Desktop\temp2.csv", SelectedStation);
        }

        public void ClearAll()
        {
            _database.ClearAll();
        }
    }
}