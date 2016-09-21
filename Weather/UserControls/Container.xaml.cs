using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for InfoPanel.xaml
    /// </summary>
    public partial class Container
    {
        private readonly ContainerViewModel _viewModel;

        public Container()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<ContainerViewModel>();
            DataContext = _viewModel;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var f = sender as ComboBox;
            var selectedItem = (IPluginWrapper) f.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }
            var t = selectedItem.Plugin.View;
            _viewModel.ContentVm = t.ViewModel;
            _viewModel.Content = t as UserControl;
        }
    }
}