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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using Weather.DependencyResolver;

namespace Weather.UserControls.Charts
{
    /// <summary>
    /// Interaction logic for LineGraph.xaml
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
            ViewModel.SelectedStation.SelectedStationsChanged -= ViewModel.SelectedStation_SelectedStationsChanged;
            ViewModel.SelectedStation.TimeSpanChanged -= ViewModel.SelectedStation_TimeSpanChanged;
            ViewModel.SelectedStation.GetRecordsCompleted -= ViewModel.SelectedStation_GetRecordsCompleted;

        }

        private void MinMax_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.SelectedStationsChanged += ViewModel.SelectedStation_SelectedStationsChanged;
            ViewModel.SelectedStation.TimeSpanChanged += ViewModel.SelectedStation_TimeSpanChanged;
            ViewModel.SelectedStation.GetRecordsCompleted += ViewModel.SelectedStation_GetRecordsCompleted;

        }
    }
}
