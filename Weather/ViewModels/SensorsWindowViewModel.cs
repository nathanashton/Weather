using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.Helpers;
using Weather.Views;
using Weather.DependencyResolver;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorsWindowViewModel
    {
        public SensorsWindow Window { get; set; }
        public ObservableCollection<ISensor> Sensors { get; set; }
        private readonly ISensorCore _sensorCore;
        private readonly ISensorTypeCore _sensorTypeCore;
        public ISensor SelectedSensor { get; set; }
        public ISensor TempSelectedSensor { get; set; }
        public ObservableCollection<ISensorType> SensorTypes { get; set; }
        public bool Adding { get; set; }
        public bool IsDirty { get; set; }

        public SensorsWindowViewModel(ISensorCore sensorCore, ISensorTypeCore sensorTypeCore)
        {
            _sensorCore = sensorCore;
            _sensorTypeCore = sensorTypeCore;
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

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete, x => SelectedSensor != null && SelectedSensor.SensorId != 0);
            }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, x=> IsDirty && SelectedSensor != null && SelectedSensor.IsValid); }
        }

        public ICommand SensorTypesCommand
        {
            get { return new RelayCommand(SensorTypesWindowOpen, x =>SelectedSensor != null); }
        }

        private void SensorTypesWindowOpen(object obj)
        {
            var id = SelectedSensor.SensorType.SensorTypeId;
            var id2 = SelectedSensor.SensorId;
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorTypesWindow>();
            window.ShowDialog();

            GetAllSensors();
            GetAllSensorTypes();

            SelectedSensor = Sensors.First(x => x.SensorId == id2);
            Window.SelectSensorInListBox(SelectedSensor);
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel, x => (IsDirty || Adding) && SelectedSensor != null); }
        }

        private void Save(object obj)
        {
            if (!SelectedSensor.IsValid) return;
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
            //if (_sensorTypeCore.AnySensorTypesUseUnit(SelectedUnit))
            //{
            //    MessageBox.Show(
            //        "Cannot delete " + SelectedUnit.DisplayName + " as it is currently being used by a Sensor Type",
            //        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}


            var result = MessageBox.Show("Delete " + SelectedSensor.ToString() + "?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Stop);
            if (result != MessageBoxResult.Yes) return;
            _sensorCore.Delete(SelectedSensor);

            SelectedSensor = null;
            TempSelectedSensor = null;
            CheckDirty();
            GetAllSensors();
        }

        private void SensorType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Description_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Model_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Manufacturer_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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
            if (SelectedSensor.SensorType == null) return;
            if (Adding)
            {
                IsDirty = true;
                return;
            }
            if (SelectedSensor == null || TempSelectedSensor == null) return;
            if (SelectedSensor.Manufacturer != TempSelectedSensor.Manufacturer || SelectedSensor.Model != TempSelectedSensor.Model || SelectedSensor.Description != TempSelectedSensor.Description ||SelectedSensor.SensorType.SensorTypeId != TempSelectedSensor.SensorType.SensorTypeId)
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
