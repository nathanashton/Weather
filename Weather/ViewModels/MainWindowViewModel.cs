using Microsoft.Practices.Unity;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Weather.Common;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel : NotifyBase
    {
        public ObservableCollection<IWeatherStation> Stations { get; set; }
        private readonly IStationCore _stationCore;

        private WeatherStation _selectedStation;

        public WeatherStation SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                OnPropertyChanged(() => SelectedStation);
                //  _stationCore.SelectedStation = value;
            }
        }

        public MainWindowViewModel(IStationCore stationCore, IImporter importer)
        {
            _stationCore = stationCore;
            Stations = new ObservableCollection<IWeatherStation>();
            //   GetAllStations();
        }

        public ICommand ImportCommand
        {
            get { return new RelayCommand(Import, x => SelectedStation != null); }
        }

        public ICommand SensorTypesCommand
        {
            get { return new RelayCommand(SensorTypes, x => true); }
        }

        public ICommand UnitsCommand
        {
            get { return new RelayCommand(Units, x => true); }
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

        private async void GetAllStations()
        {
            Stations.Clear();
            var allstations = await _stationCore.GetAllStationsAsync();
            Stations = new ObservableCollection<IWeatherStation>(allstations);
        }

        private void Import(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<ImportWindow>();
            window.ShowDialog();

            GetAllStations();
        }

        private void SensorTypes(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorTypesWindow>();
            window.ShowDialog();
        }

        private void Units(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<UnitsWindow>();
            window.ShowDialog();
        }

        public void ClearAll()
        {
            //  _database.ClearAll();
        }
    }
}