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
using Weather.DependencyResolver;
using Weather.ViewModels;
using Microsoft.Practices.Unity;


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



    }
}
