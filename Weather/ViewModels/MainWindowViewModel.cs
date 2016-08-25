using Microsoft.Practices.Unity;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Animation;
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
        private bool _debugPanelVisible;

        public bool DebugPanelVisible
        {
            get { return _debugPanelVisible; }
            set
            {
                _debugPanelVisible = value;
                if (value)
                {
                    DebugPanel = "";
                    var storyboard = MainWindow.Resources["ShowDebugPanel"] as Storyboard;
                    storyboard.Begin();
                }
                else
                {
                    var storyboard = MainWindow.Resources["CloseDebugPanel"] as Storyboard;
                    storyboard.Begin();
                }
                OnPropertyChanged(() => DebugPanelVisible);
            }
        }

        public string Clock { get; set; }
        public MainWindow MainWindow { get; set; }
        private readonly IStationCore _stationCore;
        public string DebugPanel { get; set; }
        private WeatherStation _selectedStation;
        private readonly ILog _log;

        public MainWindowViewModel(IStationCore stationCore, IImporter importer, ILog log)
        {
            _log = log;
            _log.DebugPanelMessage += _log_DebugPanelMessage;
            _stationCore = stationCore;
            Stations = new ObservableCollection<IWeatherStation>();
            //   GetAllStations();
        }

        private void _log_DebugPanelMessage(object sender, Common.EventArgs.DebugMessageArgs e)
        {
            if (!DebugPanelVisible) return;
            DebugPanel += DateTime.Now + " => " + e.Message + Environment.NewLine;
            MainWindow.scroll.ScrollToBottom();
        }

        public ObservableCollection<IWeatherStation> Stations { get; set; }

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

        public ICommand ImportCommand
        {
            get { return new RelayCommand(Import, x => SelectedStation != null); }
        }


        public ICommand SensorsWindowCommand    
        {
            get { return new RelayCommand(SensorsWindowOpen, x => true); }
        }

        public ICommand SensorTypesCommand
        {
            get { return new RelayCommand(SensorTypes, x => true); }
        }

    

        public ICommand StationsCommand
        {
            get { return new RelayCommand(OpenStations, x => true); }
        }

        public ICommand UnitsWindowCommand
        {
            get { return new RelayCommand(UnitsWindowOpen, x => true); }
        }

        public ICommand SensorTypesWindowCommand
        {
            get { return new RelayCommand(SensorTypesWindowOpen, x => true); }
        }

        private void OpenStations(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<StationWindow>();
            window.ShowDialog();

            GetAllStations();
        }

        private void UnitsWindowOpen(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<UnitsWindow>();
            window.ShowDialog();
        }

        private void SensorsWindowOpen(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorsWindow>();
            window.ShowDialog();
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
            //var container = new Resolver().Bootstrap();
            //var window = container.Resolve<SensorTypesWindow>();
            //window.ShowDialog();
        }

        private void SensorTypesWindowOpen(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorTypesWindow>();
            window.ShowDialog();
        }

      


        public void ClearAll()
        {
            //  _database.ClearAll();
        }
    }
}