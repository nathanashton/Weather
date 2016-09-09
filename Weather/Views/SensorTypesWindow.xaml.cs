using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for SensorTypesWindow.xaml
    /// </summary>
    public partial class SensorTypesWindow
    {
        private readonly SensorTypesViewModel _viewModel;

        public SensorTypesWindow(SensorTypesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += SensorTypesWindow_Loaded;
            Closing += SensorTypesWindow_Closing;
        }

        private void SensorTypesWindow_Closing(object sender, CancelEventArgs e)
        {
            // _viewModel.SelectedStation.OnStationsChanged();
        }

        private void SensorTypesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetSensorTypes();
            _viewModel.RegisterDirtyHandlers();
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox) e.Source).SelectedItem;
            if (selection == null)
            {
                return;
            }

            _viewModel.TempSelectedSensorType = selection as ISensorType;
            if (_viewModel.TempSelectedSensorType != null)
            {
                _viewModel.SelectedSensorType = new SensorType
                {
                    SensorTypeId = _viewModel.TempSelectedSensorType.SensorTypeId,
                    Name = _viewModel.TempSelectedSensorType.Name,
                    SIUnit = _viewModel.TempSelectedSensorType.SIUnit,
                    Units = _viewModel.TempSelectedSensorType.Units
                };
            }

            SelectSiUnitInComboBox(_viewModel.SelectedSensorType.SIUnit);
        }

        public void SelectUnitInListBox(ISensorType sensorType)
        {
            Lb.SelectedItem = sensorType;
        }

        public void SelectSiUnitInComboBox(Unit unit)
        {
            foreach (var item in SiUnit.Items)
            {
                var o = item as Unit;
                if ((o != null) && (o.UnitId == _viewModel.SelectedSensorType.SIUnit.UnitId))
                {
                    SiUnit.SelectedItem = item;
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