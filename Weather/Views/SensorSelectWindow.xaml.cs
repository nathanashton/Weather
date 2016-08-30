using System.Windows;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SensorSelectWindow.xaml
    /// </summary>
    public partial class SensorSelectWindow : Window
    {
        public SensorSelectWindowViewModel _viewModel;

        public SensorSelectWindow(SensorSelectWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += SensorSelectWindow_Loaded;
        }

        private void SensorSelectWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllSensors();
            if (_viewModel.StationSensor != null && _viewModel.StationSensor.Sensor != null)
            {
                foreach(var item in cb.Items)
                {
                    var t = (ISensor)item;
                    if (t.SensorId == _viewModel.StationSensor.Sensor.SensorId)
                    {
                        cb.SelectedItem = item;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}