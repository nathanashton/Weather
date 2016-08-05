using Microsoft.Win32;
using System.Windows;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow
    {
        private readonly ImportWindowViewModel _viewModel;

        public ImportWindow(ImportWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _viewModel.ReadFile(openFileDialog.FileName);
            }
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_viewModel.Records != null && _viewModel.CurrentRecord != 0)
            {
                _viewModel.Record = _viewModel.Records[(_viewModel.CurrentRecord - 1)];
            }
        }
    }
}