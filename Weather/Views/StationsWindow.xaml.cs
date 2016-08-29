using System;
using System.Windows;
using System.Windows.Controls;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for StationsWindow.xaml
    /// </summary>
    public partial class StationsWindow : Window
    {
        private StationsWindowViewModel _viewModel;

        public StationsWindow(StationsWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += StationsWindow_Loaded;
        }

        private void StationsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllStations();
            _viewModel.RegisterDirtyHandlers();
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox)e.Source).SelectedItem;
            if (selection == null) return;

            _viewModel.TempSelectedWeatherStation = selection as IWeatherStation;
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

        public void SelectStationInListBox(IWeatherStation station)
        {
            lb.SelectedItem = station;
        }
    }
}