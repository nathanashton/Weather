using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for PaletteSelector.xaml
    /// </summary>
    public partial class PaletteSelector : UserControl
    {
        public PaletteSelector()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            var viewModel = container.Resolve<PaletteSelectorViewModel>();
            DataContext = viewModel;
        }
    }
}