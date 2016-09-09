using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for SensorsWindow.xaml
    /// </summary>
    public partial class SensorsWindow
    {
        private readonly SensorsWindowViewModel _viewModel;

        public SensorsWindow(SensorsWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.Window = this;
            DataContext = _viewModel;
            Loaded += SensorsWindow_Loaded;
            Closing += SensorsWindow_Closing;
        }

        private void SensorsWindow_Closing(object sender, CancelEventArgs e)
        {
            // _viewModel.SelectedStation.OnStationsChanged();
        }

        private void SensorsWindow_Loaded(object sender, RoutedEventArgs e)
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

        public void SelectSiUnitInComboBox(ISensorType unit)
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

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}