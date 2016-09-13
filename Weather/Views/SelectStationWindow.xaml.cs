using System.Windows;
using System.Windows.Input;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SelectStationWindow.xaml
    /// </summary>
    public partial class SelectStationWindow : Window
    {
        public SelectStationWindowViewModel ViewModel { get; set; }

        public SelectStationWindow(SelectStationWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            ViewModel.Window = this;
            DataContext = ViewModel;
            Loaded += SelectStationWindow_Loaded;
        }

        private void SelectStationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.GetAllStations();
            if (ViewModel.SStation.WeatherStation != null)
            {
                ViewModel.SelectedStation = ViewModel.SStation.WeatherStation;
                if (Cb.Items == null) return;
                foreach (var item in Cb.Items)
                {
                    if (((IWeatherStation) item).WeatherStationId == ViewModel.SStation.WeatherStation.WeatherStationId)
                    {
                        Cb.SelectedItem = item;
                    }
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