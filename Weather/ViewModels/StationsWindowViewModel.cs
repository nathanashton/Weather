using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class StationsWindowViewModel
    {
        private readonly IStationCore _stationCore;

        public StationsWindow Window { get; set; }
        public ObservableCollection<IWeatherStation> WeatherStations { get; set; }
        public IWeatherStation SelectedWeatherStation { get; set; }
        public IWeatherStation TempSelectedWeatherStation { get; set; }
        public bool IsDirty { get; set; }
        public bool Adding { get; set; }
        public IStationSensor SelectedSensor { get; set; }
        public ISelectedStation SelectedStation { get; set; }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save,
                    x => IsDirty && (SelectedWeatherStation != null) && SelectedWeatherStation.IsValid);
            }
        }

        public ICommand AddSensorCommand
        {
            get { return new RelayCommand(AddSensor, x => SelectedWeatherStation != null); }
        }

        public ICommand EditSensorCommand
        {
            get { return new RelayCommand(EditSensor, x => SelectedSensor != null); }
        }

        public ICommand DeleteSensorCommand
        {
            get
            {
                return new RelayCommand(DeleteSensor, x => (SelectedSensor != null) && (SelectedWeatherStation != null));
            }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel, x => (IsDirty || Adding) && (SelectedWeatherStation != null)); }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete,
                    x => (SelectedWeatherStation != null) && (SelectedWeatherStation.WeatherStationId != 0));
            }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public ICommand MapCommand
        {
            get { return new RelayCommand(Map, x => true); }
        }

        public StationsWindowViewModel(IStationCore stationCore, ISelectedStation selectedStation)
        {
            _stationCore = stationCore;
            SelectedStation = selectedStation;
        }

        private void EditSensor(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorSelectWindow>();

            window.ViewModel.Editing = true;

            window.ViewModel.WeatherStation = SelectedWeatherStation;
            window.ViewModel.StationSensor = SelectedSensor;
            window.ViewModel.SelectedSensor = SelectedSensor.Sensor;

            window.ShowDialog();

            var s = window.ViewModel.StationSensor;
            _stationCore.UpdateStationSensor(s);

            var id = SelectedWeatherStation.WeatherStationId;
            CheckDirty();
            GetAllStations();
            SelectedWeatherStation = WeatherStations.First(x => x.WeatherStationId == id);
            Window.SelectStationInListBox(SelectedWeatherStation);
        }

        private void Save(object obj)
        {
            if (Adding)
            {
                _stationCore.Add(SelectedWeatherStation);
                if (SelectedWeatherStation.Sensors != null)
                {
                    foreach (var sensor in SelectedWeatherStation.Sensors)
                    {
                        _stationCore.AddSensorToStation(sensor, SelectedWeatherStation);
                    }
                }
            }

            if (!SelectedWeatherStation.IsValid)
            {
                return;
            }
            _stationCore.AddOrUpdate(SelectedWeatherStation);
            // _log.Debug("Saved Sensor Type");
            Adding = false;
            GetAllStations();
            IsDirty = false;
        }

        private void DeleteSensor(object obj)
        {
            var result = MessageBox.Show("Remove " + SelectedSensor.Sensor + " from this Station?", "Confirm",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            var id = SelectedWeatherStation.WeatherStationId;

            _stationCore.RemoveSensorFromStation(SelectedSensor, SelectedWeatherStation);

            CheckDirty();
            GetAllStations();

            SelectedWeatherStation = WeatherStations.First(x => x.WeatherStationId == id);
            Window.SelectStationInListBox(SelectedWeatherStation);
        }

        private void AddSensor(object obj)
        {
            var tempUnitCount = SelectedWeatherStation.Sensors.Count;
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorSelectWindow>();
            window.ViewModel.WeatherStation = SelectedWeatherStation;
            window.ShowDialog();

            //// Get Unit that was added from the above dialog.
            var unit = SelectedWeatherStation.Sensors.Count;
            if (tempUnitCount == unit)
            {
                return;
            }

            var id = SelectedWeatherStation.WeatherStationId;

            _stationCore.AddSensorToStation(SelectedWeatherStation.Sensors.Last(), SelectedWeatherStation);

            CheckDirty();
            GetAllStations();

            SelectedWeatherStation = WeatherStations.First(x => x.WeatherStationId == id);
            Window.SelectStationInListBox(SelectedWeatherStation);
        }

        private void Cancel(object obj)
        {
            if (Adding)
            {
                GetAllStations();
            }

            Window.Manufacturer.Text = TempSelectedWeatherStation.Manufacturer;
            Window.Model.Text = TempSelectedWeatherStation.Model;
            Window.Description.Text = TempSelectedWeatherStation.Description;
            Window.Latitude.Text = TempSelectedWeatherStation.Latitude.ToString();
            Window.Longitude.Text = TempSelectedWeatherStation.Longitude.ToString();

            Adding = false;
            SelectedWeatherStation = null;
            TempSelectedWeatherStation = null;
            IsDirty = false;
        }

        private void Map(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<StationMapWindow>();
            window.Latitude = SelectedWeatherStation.Latitude;
            window.Longitude = SelectedWeatherStation.Longitude;
            window.ShowDialog();

            SelectedWeatherStation.Latitude = window.Latitude;
            SelectedWeatherStation.Longitude = window.Longitude;
        }

        public void GetAllStations()
        {
            WeatherStations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
        }

        public void RegisterDirtyHandlers()
        {
            Window.Manufacturer.TextChanged += Manufacturer_TextChanged;
            Window.Model.TextChanged += Manufacturer_TextChanged;
            Window.Description.TextChanged += Manufacturer_TextChanged;
            Window.Latitude.TextChanged += Manufacturer_TextChanged;
            Window.Longitude.TextChanged += Manufacturer_TextChanged;
        }

        private void Manufacturer_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Add(object obj)
        {
            Adding = true;
            var unit = new WeatherStation();
            WeatherStations.Add(unit);
            Window.SelectStationInListBox(unit);
            Window.Manufacturer.Focus();
        }

        private void Delete(object obj)
        {
            var result = MessageBox.Show("Delete " + SelectedWeatherStation + "?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            _stationCore.Delete(SelectedWeatherStation);

            SelectedWeatherStation = null;
            TempSelectedWeatherStation = null;
            CheckDirty();
            GetAllStations();
        }

        public void CheckDirty()
        {
            if (Adding)
            {
                IsDirty = true;
                return;
            }
            if ((SelectedWeatherStation == null) || (TempSelectedWeatherStation == null))
            {
                return;
            }
            if ((SelectedWeatherStation.Manufacturer != TempSelectedWeatherStation.Manufacturer)
                || (SelectedWeatherStation.Model != TempSelectedWeatherStation.Model)
                || (SelectedWeatherStation.Description != TempSelectedWeatherStation.Description)
                || (SelectedWeatherStation.Latitude != TempSelectedWeatherStation.Latitude)
                || (SelectedWeatherStation.Longitude != TempSelectedWeatherStation.Longitude))
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }
    }
}