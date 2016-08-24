using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.Common.Units;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for UnitsPage.xaml
    /// </summary>
    public partial class UnitsPage : UserControl
    {
        private readonly UnitsViewModel _viewModel;

        public UnitsPage()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<UnitsViewModel>();
            _viewModel.UnitsWindow = this;
            DataContext = _viewModel;
            Loaded += UnitsPage_Loaded;
        }

        private void UnitsPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.RegisterDirtyHandlers();
            _viewModel.GetUnits();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox) e.Source).SelectedItem as Unit;
            if (selection == null) return;
            if (_viewModel.IsDirty)
            {
                var result =
                    MessageBox.Show(
                        "Save changes to " + DisplayName.Text + "?", "Save Changes",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.Save(null);
                }
            }

            _viewModel.SelectedUnit = selection;
        }
    }
}