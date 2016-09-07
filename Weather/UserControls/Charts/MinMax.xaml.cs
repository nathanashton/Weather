using System.Windows;
using Microsoft.Practices.Unity;
using Weather.DependencyResolver;

namespace Weather.UserControls.Charts
{
    /// <summary>
    ///     Interaction logic for MinMax.xaml
    /// </summary>
    public partial class MinMax
    {
        public MinMaxViewModel ViewModel { get; set; }

        public MinMax()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            ViewModel = container.Resolve<MinMaxViewModel>();
            DataContext = ViewModel;
            Loaded += MinMax_Loaded;
            Unloaded += MinMax_Unloaded;
        }

        private void MinMax_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationsChanged -= ViewModel.SelectedStation_SelectedStationsChanged;
            ViewModel.SelectedStation.TimeSpanChanged -= ViewModel.SelectedStation_TimeSpanChanged;
        }

        private void MinMax_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationsChanged += ViewModel.SelectedStation_SelectedStationsChanged;
            ViewModel.SelectedStation.TimeSpanChanged += ViewModel.SelectedStation_TimeSpanChanged;
        }

    }
}