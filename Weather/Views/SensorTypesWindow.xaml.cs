using System.Windows;
using System.Windows.Controls;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for SensorTypesWindow.xaml
    /// </summary>
    public partial class SensorTypesWindow : Window
    {
        private readonly SensorTypesViewModel _viewModel;

        public SensorTypesWindow(SensorTypesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += SensorTypesWindow_Loaded;
        }

        private void SensorTypesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetSensorTypes();
            _viewModel.RegisterDirtyHandlers();
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox)e.Source).SelectedItem;
            if (selection == null) return;

            _viewModel.TempSelectedSensorType = selection as ISensorType;
            _viewModel.SelectedSensorType = new SensorType
            {
                SensorTypeId = _viewModel.TempSelectedSensorType.SensorTypeId,
                Name = _viewModel.TempSelectedSensorType.Name,
                SIUnit = _viewModel.TempSelectedSensorType.SIUnit,
                Units = _viewModel.TempSelectedSensorType.Units
            };

            SelectSiUnitInComboBox(_viewModel.SelectedSensorType.SIUnit);
        }

        public void SelectUnitInListBox(ISensorType sensorType)
        {
            lb.SelectedItem = sensorType;
        }

        public void SelectSiUnitInComboBox(Unit unit)
        {
            foreach (var item in SIUnit.Items)
            {
                var o = item as Unit;
                if (o != null && o.UnitId == _viewModel.SelectedSensorType.SIUnit.UnitId)
                {
                    SIUnit.SelectedItem = item;
                }
            }
        }
    }
}