using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for SelectSensor.xaml
    /// </summary>
    public partial class SelectSensor : UserControl
    {
        public SensorSelectWindowViewModel ViewModel { get; set; }


        public SelectSensor(SensorSelectWindowViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = ViewModel;
            Loaded += SelectSensor_Loaded;
        }

        private void SelectSensor_Loaded(object sender, RoutedEventArgs e)
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
            DialogHost.CloseDialogCommand.Execute("cancel", null);
        }
    }
}