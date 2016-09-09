using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Weather.DependencyResolver;

namespace Weather.UserControls.Charts
{
    /// <summary>
    ///     Interaction logic for LineGraph.xaml
    /// </summary>
    public partial class LineGraph : UserControl
    {
        public LineGraphViewModel ViewModel { get; set; }

        public LineGraph()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            ViewModel = container.Resolve<LineGraphViewModel>();
            DataContext = ViewModel;
            Loaded += MinMax_Loaded;
            Unloaded += MinMax_Unloaded;
        }

        private void MinMax_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationRecordsUpdated -= ViewModel.SelectedStationRecordsSelectedStationRecordsUpdated;
            ViewModel.SelectedStation.GetRecordsCompleted -= ViewModel.SelectedStation_GetRecordsCompleted;
        }

        private void MinMax_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationRecordsUpdated += ViewModel.SelectedStationRecordsSelectedStationRecordsUpdated;
            ViewModel.SelectedStation.GetRecordsCompleted += ViewModel.SelectedStation_GetRecordsCompleted;
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           ViewModel.SelectedSensor = null;

        }

        private void Label_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
          ViewModel.SelectedSensor2 = null;

        }
    }
}