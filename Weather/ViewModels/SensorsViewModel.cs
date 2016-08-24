using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.UserControls;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorsViewModel
    {
        public SensorsPage SensorsWindow { get; set; }
        private readonly ISensorTypeCore _sensorTypeCore;
        public ObservableCollection<ISensor> Sensors { get; set; }
        public ISensor SelectedSensor { get; set; }
        public ObservableCollection<ISensorType> SensorTypes { get; set; }
        public bool IsDirty { get; set; }

        public SensorsViewModel(ISensorTypeCore sensorTypeCore)
        {
            _sensorTypeCore = sensorTypeCore;
            Sensors = new ObservableCollection<ISensor>();
            GetAllSensorTypes();
            GetAllSensors();
            SelectedSensor = Sensors.Count == 0 ? null : Sensors.First();
        }

        public void RegisterDirtyHandlers()
        {
            SensorsWindow.Manufacturer.TextChanged += Manufacturer_TextChanged;
            SensorsWindow.Model.TextChanged += Model_TextChanged;
            SensorsWindow.Description.TextChanged += Description_TextChanged;
            SensorsWindow.SensorType.SelectionChanged += SensorType_SelectionChanged;
        }

        private void SensorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedSensor == null) return;
            if (SensorsWindow.SensorType.SelectedItem != SelectedSensor.SensorType)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedSensor == null) return;
            if (SensorsWindow.Description.Text != SelectedSensor.Description)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        private void Model_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedSensor == null) return;
            if (SensorsWindow.Model.Text != SelectedSensor.Model)
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
            if (SelectedSensor == null) return;
            if (SensorsWindow.Manufacturer.Text != SelectedSensor.Manufacturer)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        public void GetAllSensors()
        {
            Sensors.Clear();
            var sensor1 = new Sensor
            {
                Manufacturer = "Oregon",
                Model = "LW201",
                SensorType = SensorTypes.First()
            };
            var sensor2 = new Sensor
            {
                Manufacturer = "David",
                Model = "1234",
                SensorType = SensorTypes.Last()
            };
            Sensors.Add(sensor1);
            Sensors.Add(sensor2);
        }

        private void GetAllSensorTypes()
        {
            SensorTypes = new ObservableCollection<ISensorType>(_sensorTypeCore.GetAll());
        }

        public bool Validate(ISensor sensor)
        {
            return !string.IsNullOrEmpty(sensor.Manufacturer) && !string.IsNullOrEmpty(sensor.Model) && sensor.SensorType != null;
        }

        public void Save(object obj)
        {
            // We need to create a temp object to validate against as the bindings haven't been committed yet.
            var tempSensor = new Sensor
            {
                Manufacturer = SensorsWindow.Manufacturer.Text,
                Model = SensorsWindow.Model.Text,
                SensorType = (ISensorType)SensorsWindow.SensorType.SelectedItem,
                Description = SensorsWindow.Description.Text
            };
            if (!Validate(tempSensor))
            {
                MessageBox.Show("Sensor not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var manufacturer = SensorsWindow.Manufacturer.GetBindingExpression(TextBox.TextProperty);
            manufacturer?.UpdateSource();
            var model = SensorsWindow.Model.GetBindingExpression(TextBox.TextProperty);
            model?.UpdateSource();
            var sensorType = SensorsWindow.SensorType.GetBindingExpression(Selector.SelectedItemProperty);
            sensorType?.UpdateSource();
            var secription = SensorsWindow.Description.GetBindingExpression(TextBox.TextProperty);
            secription?.UpdateSource();

            //  _sensorTypeCore.AddOrUpdate(SelectedSensorType);

            IsDirty = false;
            GetAllSensors();
        }
    }
}