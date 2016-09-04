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
using Weather.ViewModels;
using Weather.DependencyResolver;
using Microsoft.Practices.Unity;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for InfoPanel.xaml
    /// </summary>
    public partial class Container : UserControl
    {

        private ContainerViewModel _viewModel;

        public Container()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<ContainerViewModel>();
            DataContext = _viewModel;
        }
    }
}
