using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Core;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;
using Xceed.Wpf.Toolkit.Primitives;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class StationWindowViewModel
    {
        private readonly ISensorCore _sensorCore;
        private readonly IStationCore _stationCore;

        public StationWindowViewModel(IStationCore stationCore, ISensorCore sensorCore)
        {
             _stationCore = stationCore;
            _stationCore.StationsChanged += _stationCore_StationsChanged;
            _sensorCore = sensorCore;
            GetAllStations();
            SelectedStation = Stations.FirstOrDefault();
        }

        private void _stationCore_StationsChanged(object sender, System.EventArgs e)
        {
            Stations = _stationCore.Stations;
        }

        public ObservableCollection<WeatherStation> Stations { get; set; }
        public WeatherStation SelectedStation { get; set; }
        public Sensor SelectedSensor { get; set; }
        public StationWindow StationWindow { get; set; }
        public bool IsDirty { get; set; }

        public ICommand DeleteStationCommand
        {
            get { return new RelayCommand(DeleteStation, x => SelectedStation != null); }
        }

        public ICommand AddStationCommand
        {
            get { return new RelayCommand(AddStation, x => true); }
        }

        public ICommand SaveStationCommand
        {
            get { return new RelayCommand(SaveStation, x => SelectedStation != null && IsDirty); }
        }

        public ICommand DeleteSensorCommand
        {
            get { return new RelayCommand(DeleteSensor, x => SelectedSensor != null); }
        }

        public ICommand EditSensorCommand
        {
            get { return new RelayCommand(EditSensor, x => SelectedSensor != null); }
        }

        public ICommand AddSensorCommand
        {
            get { return new RelayCommand(AddSensor, x => SelectedStation != null && SelectedStation.Id != 0); }
        }

        public ICommand MapCommand
        {
            get { return new RelayCommand(Map, x => SelectedStation != null); }
        }

        public void GetAllStations()
        {
            _stationCore.GetAllStations();
            Stations = _stationCore.Stations;
        }

        private void DeleteStation(object obj)
        {
            var result = MessageBox.Show(
                "Are you sure you wish to delete the " + SelectedStation.Manufacturer + " " +
                SelectedStation.Model +
                " station? This will delete all sensors and associated recordings for this station.", "Delete?",
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result != MessageBoxResult.Yes) return;
            _stationCore.DeleteStation(SelectedStation);
            SelectedStation = Stations.FirstOrDefault();
        }

        private void AddStation(object obj)
        {
            var newStation = new WeatherStation {Manufacturer = "unnamed"};
            _stationCore.AddStation(newStation);
            SelectedStation = newStation;
            IsDirty = true;
        }

        private void SaveStation(object obj)
        {
            var textManufacturer = StationWindow.Manufacturer.GetBindingExpression(TextBox.TextProperty);
            textManufacturer?.UpdateSource();
            var textModel = StationWindow.Model.GetBindingExpression(TextBox.TextProperty);
            textModel?.UpdateSource();
            var textLatitude = StationWindow.Latitude.GetBindingExpression(InputBase.TextProperty);
            textLatitude?.UpdateSource();
            var textLongitude = StationWindow.Longitude.GetBindingExpression(InputBase.TextProperty);
            textLongitude?.UpdateSource();
            SelectedStation = _stationCore.Update(SelectedStation);
            IsDirty = false;
        }

        public void SaveSpecificStation(WeatherStation station)
        {
            var textManufacturer = StationWindow.Manufacturer.GetBindingExpression(TextBox.TextProperty);
            textManufacturer?.UpdateSource();
            var textModel = StationWindow.Model.GetBindingExpression(TextBox.TextProperty);
            textModel?.UpdateSource();
            var textLatitude = StationWindow.Latitude.GetBindingExpression(InputBase.TextProperty);
            textLatitude?.UpdateSource();
            var textLongitude = StationWindow.Longitude.GetBindingExpression(InputBase.TextProperty);
            textLongitude?.UpdateSource();
            station = _stationCore.Update(station);
            IsDirty = false;
        }

        public void RegisterFirtyHandlers()
        {
            StationWindow.Manufacturer.TextChanged += Manufacturer_TextChanged;
            StationWindow.Model.TextChanged += Manufacturer_TextChanged;
            StationWindow.Latitude.ValueChanged += Latitude_ValueChanged;
            StationWindow.Longitude.ValueChanged += Latitude_ValueChanged;
        }

        private void Latitude_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedStation == null)
            {
                return;
            }
            if (!StationWindow.Longitude.Value.Equals(SelectedStation.Longitude) ||
                !StationWindow.Latitude.Value.Equals(SelectedStation.Latitude))
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        private void Manufacturer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedStation == null) return;
            if (StationWindow.Manufacturer.Text != SelectedStation.Manufacturer ||
                StationWindow.Model.Text != SelectedStation.Model)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        private void AddSensor(object obj)
        {
            var container = Resolver.Bootstrap();
            var window = container.Resolve<SensorWindow>();
            window.ShowDialog();

            var sensor = window.ViewModel.Sensor;
            if (sensor == null) return;
            sensor.Station = SelectedStation;
            sensor = _sensorCore.AddSensor(sensor);
            SelectedStation.AddSensor(sensor);
        }

        private void DeleteSensor(object obj)
        {
            var result =
                MessageBox.Show(
                    "Are you sure you wish to delete the " + SelectedSensor.Name +
                    " sensor?. This will delete ALL sensor recordings as well.", "Delete?", MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation);
            if (result != MessageBoxResult.Yes) return;
            _sensorCore.DeleteSensor(SelectedSensor);
            _sensorCore.DeleteSensor(SelectedSensor);
            SelectedStation.Sensors.Remove(SelectedSensor);
        }

        public void EditSensor(object obj)
        {
            var container = Resolver.Bootstrap();
            var window = container.Resolve<SensorWindow>();

            window.ViewModel.EditSensor = SelectedSensor;
            window.ViewModel.EditSensor.Station = SelectedStation;
            window.ShowDialog();


            var sensor = window.ViewModel.Sensor;
            if (sensor == null) return;

            SelectedStation.Sensors.Remove(SelectedSensor);
            SelectedStation.AddSensor(sensor);

            _sensorCore.UpdateSensorForWeatherStation(sensor);
        }

        private void Map(object obj)
        {
            var container = Resolver.Bootstrap();
            var window = container.Resolve<StationMapWindow>();
            window.Latitude = SelectedStation.Latitude;
            window.Longitude = SelectedStation.Longitude;
            window.ShowDialog();

            SelectedStation.Latitude = window.Latitude;
            SelectedStation.Longitude = window.Longitude;
        }

        public void SelectedStationChanged()
        {
            MessageBox.Show(IsDirty.ToString());
        }
    }
}