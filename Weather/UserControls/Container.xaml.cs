using Microsoft.Practices.Unity;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for InfoPanel.xaml
    /// </summary>
    public partial class Container
    {
        public Container()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            var viewModel = container.Resolve<ContainerViewModel>();
            DataContext = viewModel;
        }
    }
}