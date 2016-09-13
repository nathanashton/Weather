using System.Windows;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for StationSidePanel.xaml
    /// </summary>
    [ImplementPropertyChanged]
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
            if (Application.Current.MainWindow != null)
            {
                _viewModel.GetAllStations();
            }
        }
    }
}