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
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for SidePanel.xaml
    /// </summary>
    public partial class SidePanel : UserControl
    {

        private readonly StationPanelViewModel _viewModel;

        public SidePanel()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<StationPanelViewModel>();
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
