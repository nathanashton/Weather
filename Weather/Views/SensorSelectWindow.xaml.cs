using System.Windows;
using System.Windows.Input;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for SensorSelectWindow.xaml
    /// </summary>
    public partial class SensorSelectWindow
    {
        public SensorSelectWindowViewModel ViewModel;

        public SensorSelectWindow(SensorSelectWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
            ViewModel.Window = this;
            Loaded += SensorSelectWindow_Loaded;
        }

        private void SensorSelectWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.GetAllSensors();
            if (ViewModel.StationSensor?.Sensor != null)
            {
                foreach (var item in Cb.Items)
                {
                    var t = (ISensor) item;
                    if (t.SensorId == ViewModel.StationSensor.Sensor.SensorId)
                    {
                        Cb.SelectedItem = item;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}