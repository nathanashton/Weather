using System.Windows;
using System.Windows.Controls;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SensorTypesWindow.xaml
    /// </summary>
    public partial class SensorTypesWindow : Window
    {
        private readonly SensorTypeWindowViewModel _viewModel;

        public SensorTypesWindow(SensorTypeWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.SensorTypesWindow = this;
            Loaded += SensorTypesWindow_Loaded;
        }

        private void SensorTypesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.RegisterDirtyHandlers();
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (_viewModel.SelectedSensorType == null) return;
            var selection = ((ListBox)e.Source).SelectedItem as ISensorType;
            if (selection == null) return;
            if (_viewModel.IsDirty)
            {
                var result =
                    MessageBox.Show(
                        "Save changes to " + _viewModel.SelectedSensorType.Name + "?", "Save Changes", MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.Save(null);
                }
            }

            _viewModel.SelectedSensorType = selection;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}