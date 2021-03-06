﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Helpers;
using Weather.UserControls;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorsWindowViewModel
    {
        private readonly ISensorCore _sensorCore;
        private readonly ISensorTypeCore _sensorTypeCore;
        private readonly IStationCore _stationCore;
        public ISelectedStation SelectedStation;

        public Sensors Window { get; set; }

        public ObservableCollection<ISensor> Sensors { get; set; }
        public ISensor SelectedSensor { get; set; }
        public ISensor TempSelectedSensor { get; set; }
        public ObservableCollection<ISensorType> SensorTypes { get; set; }
        public bool Adding { get; set; }
        public bool IsDirty { get; set; }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(Delete, x => (SelectedSensor != null) && (SelectedSensor.SensorId != 0)); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, x => IsDirty && (SelectedSensor != null) && SelectedSensor.IsValid); }
        }

        public ICommand SensorTypesCommand
        {
            get { return new RelayCommand(SensorTypesWindowOpen, x => SelectedSensor != null); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel, x => (IsDirty || Adding) && (SelectedSensor != null)); }
        }

        public SensorsWindowViewModel(ISensorCore sensorCore, ISensorTypeCore sensorTypeCore, IStationCore stationCore,
            ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            _sensorCore = sensorCore;
            _sensorTypeCore = sensorTypeCore;
            _stationCore = stationCore;
            Sensors = new ObservableCollection<ISensor>();
            SensorTypes = new ObservableCollection<ISensorType>();
        }

        public void RegisterDirtyHandlers()
        {
            Window.Manufacturer.TextChanged += Manufacturer_TextChanged;
            Window.Model.TextChanged += Model_TextChanged;
            Window.Description.TextChanged += Description_TextChanged;
            Window.SensorType.SelectionChanged += SensorType_SelectionChanged;
        }

        private void SensorTypesWindowOpen(object obj)
        {
            //var id2 = SelectedSensor.SensorId;
            //var container = new Resolver().Bootstrap();
            //var window = container.Resolve<SensorTypesWindow>();
            //window.ShowDialog();

            //GetAllSensors();
            //GetAllSensorTypes();

            //SelectedSensor = Sensors.First(x => x.SensorId == id2);
            //Window.SelectSensorInListBox(SelectedSensor);
        }

        private void Save(object obj)
        {
            if (!SelectedSensor.IsValid)
            {
                return;
            }
            _sensorCore.AddOrUpdate(SelectedSensor);
            Adding = false;
            GetAllSensors();
            IsDirty = false;
        }

        private void Add(object obj)
        {
            Adding = true;
            var unit = new Sensor();
            Sensors.Add(unit);
            Window.SelectSensorInListBox(unit);
            Window.Manufacturer.Focus();
        }

        private void Cancel(object obj)
        {
            if (Adding)
            {
                GetAllSensors();
            }
            Window.Manufacturer.Text = TempSelectedSensor.Manufacturer;
            Window.Model.Text = TempSelectedSensor.Model;
            Window.Description.Text = TempSelectedSensor.Description;
            Window.SelectSiUnitInComboBox(TempSelectedSensor.SensorType);
            Adding = false;
            SelectedSensor = null;
            TempSelectedSensor = null;
            IsDirty = false;
        }

        private void Delete(object obj)
        {
            var used = _stationCore.AnyStationUsesSensor(SelectedSensor);
            if (used)
            {
                MessageBox.Show("Cannot delete " + SelectedSensor + " as it is used in a Weather Station", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Delete " + SelectedSensor + "?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Stop);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            _sensorCore.Delete(SelectedSensor);

            SelectedSensor = null;
            TempSelectedSensor = null;
            CheckDirty();
            GetAllSensors();
        }

        private void SensorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Model_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Manufacturer_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        public void GetAllSensors()
        {
            Sensors = new ObservableCollection<ISensor>(_sensorCore.GetAllSensors());
        }

        public void GetAllSensorTypes()
        {
            SensorTypes = new ObservableCollection<ISensorType>(_sensorTypeCore.GetAll());
        }

        public void CheckDirty()
        {
            if (SelectedSensor?.SensorType == null)
            {
                return;
            }
            if (Adding)
            {
                IsDirty = true;
                return;
            }
            if ((SelectedSensor == null) || (TempSelectedSensor == null))
            {
                return;
            }
            if ((SelectedSensor.Manufacturer != TempSelectedSensor.Manufacturer) ||
                (SelectedSensor.Model != TempSelectedSensor.Model) ||
                (SelectedSensor.Description != TempSelectedSensor.Description) ||
                (SelectedSensor.SensorType.SensorTypeId != TempSelectedSensor.SensorType.SensorTypeId))
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