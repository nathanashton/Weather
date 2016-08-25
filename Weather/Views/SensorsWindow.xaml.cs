using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SensorsWindow.xaml
    /// </summary>
    public partial class SensorsWindow : Window
    {
        private readonly SensorsWindowViewModel _viewModel;

        public SensorsWindow(SensorsWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.Window = this;
            DataContext = _viewModel;
            Loaded += SensorsWindow_Loaded;
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
            if (selection == null) return;

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
            lb.SelectedItem = sensor;
        }

        public void SelectSiUnitInComboBox(ISensorType unit)
        {
            foreach (var item in SensorType.Items)
            {
                var o = item as SensorType;
                if (o != null && o.SensorTypeId == _viewModel.SelectedSensor.SensorType.SensorTypeId)
                {
                    SensorType.SelectedItem = item;
                }
            }
        }
    }
}
