using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for Stations.xaml
    /// </summary>
    public partial class Stations : UserControl
    {
        private readonly StationsWindowViewModel _viewModel;

        public Stations()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<StationsWindowViewModel>();
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += Stations_Loaded;
        }

        private void Stations_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.DialogHost = x;
            _viewModel.GetAllStations();
            _viewModel.RegisterDirtyHandlers();
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox) e.Source).SelectedItem;
            if (selection == null)
            {
                return;
            }

            _viewModel.TempSelectedWeatherStation = selection as IWeatherStation;
            if (_viewModel.TempSelectedWeatherStation != null)
            {
                _viewModel.SelectedWeatherStation = new WeatherStation
                {
                    WeatherStationId = _viewModel.TempSelectedWeatherStation.WeatherStationId,
                    Manufacturer = _viewModel.TempSelectedWeatherStation.Manufacturer,
                    Model = _viewModel.TempSelectedWeatherStation.Model,
                    Latitude = _viewModel.TempSelectedWeatherStation.Latitude,
                    Longitude = _viewModel.TempSelectedWeatherStation.Longitude,
                    Description = _viewModel.TempSelectedWeatherStation.Description,
                    Sensors = _viewModel.TempSelectedWeatherStation.Sensors
                };
            }
        }

        public void SelectStationInListBox(IWeatherStation station)
        {
            Lb.SelectedItem = station;
        }
    }
}