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
            // ViewModel.SelectedStation.OnStationsChanged();
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
                    UnitType = _viewModel.TempSelectedSensorType.UnitType
                };
            }
        }

        public void SelectUnitInListBox(ISensorType sensorType)
        {
            Lb.SelectedItem = sensorType;
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