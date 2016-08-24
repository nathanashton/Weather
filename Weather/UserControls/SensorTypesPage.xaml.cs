using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for SensorTypesPage.xaml
    /// </summary>
    public partial class SensorTypesPage : UserControl
    {
        private readonly SensorTypesViewModel _viewModel;

        public SensorTypesPage()
        {
            //InitializeComponent();
            //var container = new Resolver().Bootstrap();
            //_viewModel = container.Resolve<SensorTypesViewModel>();
            //DataContext = _viewModel;
            //_viewModel.SensorTypesWindow = this;
            //Loaded += SensorTypesPage_Loaded;
        }

        private void SensorTypesPage_Loaded(object sender, RoutedEventArgs e)
        {
            //_viewModel.RegisterDirtyHandlers();
            //_viewModel.GetSensorTypes();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var selection = ((ListBox) e.Source).SelectedItem as ISensorType;
            //if (selection == null) return;
            //if (_viewModel.IsDirty)
            //{
            //    var result =
            //        MessageBox.Show(
            //            "Save changes to " + Name.Text + "?", "Save Changes",
            //            MessageBoxButton.YesNo,
            //            MessageBoxImage.Question);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        _viewModel.Save(null);
            //    }
            //}
            //_viewModel.SelectedSensorType = selection;
        }
    }
}