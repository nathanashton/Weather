using Microsoft.Practices.Unity;
using System.Windows.Controls;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for ChartPanel.xaml
    /// </summary>
    public partial class ChartPanel : UserControl
    {
        private ChartPanelViewModel _viewModel;

        public ChartPanel()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<ChartPanelViewModel>();
            _viewModel.Window = this;
            DataContext = _viewModel;
        }
    }
}