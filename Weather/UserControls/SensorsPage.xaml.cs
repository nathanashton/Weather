using System.Windows;
using Microsoft.Practices.Unity;
using System.Windows.Controls;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for SensorsPage.xaml
    /// </summary>
    public partial class SensorsPage : UserControl
    {
        private readonly SensorsViewModel _viewModel;

        public SensorsPage()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<SensorsViewModel>();
            DataContext = _viewModel;
            _viewModel.SensorsWindow = this;
            Loaded += SensorsPage_Loaded;
        }

        private void SensorsPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllSensors();
            _viewModel.RegisterDirtyHandlers();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.SelectedSensor == null) return;
            var selection = ((ListBox)e.Source).SelectedItem as ISensor;
            if (selection == null) return;
            if (_viewModel.IsDirty)
            {
                var result =
                    MessageBox.Show(
                        "Save changes to " + Manufacturer.Text + " " + Model.Text + "?", "Save Changes",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.Save(null);
                }
            }

            _viewModel.SelectedSensor = selection;
        }
    }
}