using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for SidePanel.xaml
    /// </summary>
    public partial class SidePanel : UserControl
    {
        private readonly SidePanelViewModel _viewModel;

        public SidePanel()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<SidePanelViewModel>();
            DataContext = _viewModel;
            Loaded += SidePanel_Loaded;
        }

        private void SidePanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
            {
                _viewModel.GetAllStations();
            }
        }
    }
}