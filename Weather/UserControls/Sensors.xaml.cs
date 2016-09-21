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
    ///     Interaction logic for Sensors.xaml
    /// </summary>
    public partial class Sensors : UserControl
    {
        private readonly SensorsWindowViewModel _viewModel;

        public Sensors()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<SensorsWindowViewModel>();
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += Sensors_Loaded;
        }

        private void Sensors_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllSensors();
            _viewModel.GetAllSensorTypes();
            _viewModel.RegisterDirtyHandlers();
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox) e.Source).SelectedItem;
            if (selection == null)
            {
                return;
            }

            _viewModel.TempSelectedSensor = selection as ISensor;
            _viewModel.SelectedSensor = new Sensor
            {
                SensorId = _viewModel.TempSelectedSensor.SensorId,
                Manufacturer = _viewModel.TempSelectedSensor.Manufacturer,
                Model = _viewModel.TempSelectedSensor.Model,
                Description = _viewModel.TempSelectedSensor.Description,
                SensorType = _viewModel.TempSelectedSensor.SensorType
            };
            if (_viewModel.SelectedSensor.SensorType != null)
            {
                SelectSiUnitInComboBox(_viewModel.SelectedSensor.SensorType);
            }
        }

        public void SelectSensorInListBox(ISensor sensor)
        {
            Lb.SelectedItem = sensor;
        }

        public
            void SelectSiUnitInComboBox
            (ISensorType
                unit)
        {
            foreach (var item in SensorType.Items)
            {
                var o = item as SensorType;
                if ((o != null) && (o.SensorTypeId == _viewModel.SelectedSensor.SensorType.SensorTypeId))
                {
                    SensorType.SelectedItem = item;
                }
            }
        }
    }
}