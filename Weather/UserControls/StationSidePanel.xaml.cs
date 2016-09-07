using System.Windows;
using Microsoft.Practices.Unity;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for StationSidePanel.xaml
    /// </summary>
    public partial class StationSidePanel
    {
        private readonly StationPanelViewModel _viewModel;

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
    }
}