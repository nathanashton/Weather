using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Controls;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for StationSidePanel.xaml
    /// </summary>
    public partial class StationSidePanel : UserControl
    {
        private StationPanelViewModel _viewModel;

        public StationSidePanel()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<StationPanelViewModel>();
            DataContext = _viewModel;
            Loaded += StationSidePanel_Loaded;
        }

        private void StationSidePanel_Loaded(object sender, RoutedEventArgs e)
        {
 _viewModel.GetAllStations();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}