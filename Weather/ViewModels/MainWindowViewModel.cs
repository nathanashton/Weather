using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Weather.Common;
using Weather.Common.Entities;
using Weather.Core;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel : NotifyBase
    {
        public ObservableCollection<WeatherStation> Stations { get; set; }
        private readonly IStationCore _stationCore;
        private readonly IImporter _importer;
        private readonly Database _database;

        private WeatherStation _selectedStation;

        public WeatherStation SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                OnPropertyChanged(() => SelectedStation);
                _stationCore.SelectedStation = value;
            }
        }

        public ICommand ImportCommand
        {
            get { return new RelayCommand(Import, x => SelectedStation !=null); }
        }

        public ICommand StationsCommand
        {
            get { return new RelayCommand(OpenStations, x => true); }
        }


        private void OpenStations(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<StationWindow>();
            window.ShowDialog();

            GetAllStations();
        }

        public MainWindowViewModel(IStationCore stationCore, IImporter importer, Database database)
        {
            _stationCore = stationCore;
            _importer = importer;
            _database = database;
            Stations = new ObservableCollection<WeatherStation>();
            GetAllStations();
        }


        private void GetAllStations()
        {
            Stations.Clear();
            var all = _stationCore.GetAllStations();
            foreach (var station in all)
            {
                Stations.Add(station);
            }
        }

        private void Import(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<ImportWindow>();
            window.ShowDialog();

            GetAllStations();
        }

        public void ClearAll()
        {
            _database.ClearAll();
        }
    }
}